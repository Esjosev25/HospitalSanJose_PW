const UserChanged = () => {
    const URL = "/Users/GetUserJson";
    $.post(URL, {
        "UserId": $("#UserId option:selected").val()
    }, (data) => {
        document.getElementById("User_FirstName").value = data.user.firstName;
        document.getElementById("User_LastName").value = data.user.lastName;
        document.getElementById("User_Email").value = data.user.email;
        RolesByUser($("#UserId option:selected").val());
    });
}

const RolesByUser = (userId) => {

    const URLCity = "/Roles/GetAvailableRolesForUserJson";
    $.post(URLCity, {
        userId
    }, (data) => {
        console.log(data);
        if (!data) data = []
        options = data.map(function (r) {
            return {
                value: r.id,
                text: r.name
            }
        });

        console.log(options);
        var select, $option;

        select = $("#RoleId");
        select.empty();

        $.each(options, function (index, option) {
            $option = $("<option></option>")
                .attr("value", option.value)
                .text(option.text);
            select.append($option);
        });
    });
}