const UserChanged = () => {
    const URL = "/Users/GetUserJson";
    $.post(URL, {
        "UserId": $("#UserId option:selected").val()
    }, (data) => {
        console.log(data);
        document.getElementById("User_FirstName").value = data.user.firstName;
        document.getElementById("User_LastName").value = data.user.lastName;
        document.getElementById("User_Username").value = data.user.username;
    });
}