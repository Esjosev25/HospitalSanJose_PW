const UserChanged = (userId) => {
    const URL = "/Users/GetUserByDoctorIdJson";
    $.post(URL, {
        "DoctorId": $("#DoctorId option:selected").val()
    }, (data) => {
        document.getElementById("Doctor_User_FirstName").value = data.user.firstName;
        document.getElementById("Doctor_User_LastName").value = data.user.lastName;
        document.getElementById("Doctor_User_Email").value = data.user.email;
        DepartmentsByUser($("#DoctorId option:selected").val());
    });
}

const DepartmentsByUser = (doctorId) => {

    const URLDepartment = "/Departments/GetAvailableDepartmentsForDoctorJson";
    $.post(URLDepartment, {
        doctorId
    }, (data) => {
        
        if (!data) data = []
        options = data.map(function (r) {
            return {
                value: r.id,
                text: r.departmentName
            }
        });

        
        var select, $option;

        select = $("#DepartmentId");
        select.empty();

        $.each(options, function (index, option) {
            $option = $("<option></option>")
                .attr("value", option.value)
                .text(option.text);
            select.append($option);
        });
    });
}