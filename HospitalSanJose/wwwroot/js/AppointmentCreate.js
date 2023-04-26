const getDateNow=()=>{
    let currentDate = new Date();
    let year = currentDate.getFullYear();
    let month = String(currentDate.getMonth() + 1).padStart(2, '0');
    let day = String(currentDate.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;

}

const DepartmentChanged = () => {
    const URL = "/Doctors/GetDoctorsByDepartmentJson";
    $.post(URL, {
        "DepartmentId": $("#DepartmentId option:selected").val()
    }, (data) => {
        if (!data) data = []
        options = data.map(function (r) {
            return {
                value: r.id,
                text: `${r.user.firstName} ${r.user.lastName} (${r.user.username})`
            }
        });

        var select, $option;

        select = $("#DoctorId");
        select.empty();

        $.each(options, function (index, option) {
            $option = $("<option></option>")
                .attr("value", option.value)
                .text(option.text);
            select.append($option);
        });
        document.getElementById("AppointmentDate").value = getDateNow();
        DateTimeChange()
    });
}

const DateTimeChange = (doctorId) => {
    date =  $("#AppointmentDate").val() || getDateNow();
    doctorId = doctorId || $("#DoctorId option:selected").val()
    const URL = "/Appointments/GetAvailableHoursByDoctor";
    $.post(URL, {
        "AppointmentDate": date,
        "DoctorId": doctorId
    }, (data) => {
        
        if (!data) data = []
        options = data.map(function (r) {
            return {
                value: r,
                text: r
            }
        });

        
        var select, $option;

        select = $("#AppointmentTime");
        select.empty();

        $.each(options, function (index, option) {
            $option = $("<option></option>")
                .attr("value", option.value)
                .text(option.text);
            select.append($option);
        });
    });
}
