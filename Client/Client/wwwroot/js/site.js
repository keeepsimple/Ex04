// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
import {TOKEN, WEB_URL} from './config.js'

function checkLogin() {
    let decodeToken = "";
    if (!TOKEN) {
        $('.logined').addClass("hide")
        $('.notLogin').removeClass("hide")
    } else {
        $('.notLogin').addClass("hide")
        $('.logined').removeClass("hide")
        let parseToken = parseJwt(TOKEN)
        decodeToken = {
            "UserId" : parseToken["UserId"],
            "Username" : parseToken["Username"],
            "Roles": parseToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"]
        }
        localStorage.setItem("deToken", JSON.stringify(decodeToken))
    }
    $('#loginUsername').text('Hello ' + decodeToken.Username)
    if (TOKEN && decodeToken.Roles === "Admin") {
        $('#management').removeClass('hide')
    } else {
        $('#management').addClass('hide')
    }

}

checkLogin()

$('#logout').submit(() => {
    window.location.replace(WEB_URL)
    localStorage.removeItem('token');
    localStorage.removeItem("deToken")
})

function parseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}