const notifySuccess = (message) => {
    $(".notify").toggleClass("active");
    $(".notify").toggleClass("success");
    $("#notifyType").html(message);

    setTimeout(function () {
        $(".notify").removeClass("active");
        $(".notify").removeClass("success");
        $("#notifyType").html("");
    }, 2000);
}
const notifyError = (message) => {
    $(".notify").addClass("active");
    $(".notify").addClass("failure");
    $("#notifyType").html(message);

    setTimeout(function () {
        $(".notify").removeClass("active");
        $(".notify").removeClass("failure");
        $("#notifyType").html("");
    }, 2000);
}