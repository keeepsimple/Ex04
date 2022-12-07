import {REGISTER_API_URL, WEB_URL} from './config.js'

$('#register').submit(function (e) {
    let email = $('#email').val()
    let username = $('#username').val()
    let password = $('#password').val()
    e.preventDefault();
    axios({
        method: 'post',
        url: REGISTER_API_URL,
        data: {
            username: username,
            email: email,
            password: password
        }
    }).then(res=> {
        window.location.replace(WEB_URL+'/Authen/Login')
    }).catch(err => {
        $('#error').text(err.response.data.mess)
    })
})

$('#confirmPassword').keyup(function () {
    let password = $('#password').val()
    let confirmPass = $('#confirmPassword').val()
    if (password == confirmPass) {
        $('#registerSubmit').attr('disabled', false)
        $('#message').text("")
    } else {
        $('#registerSubmit').attr('disabled', true)
        $('#message').text("Password and Confirm password not match")
    }
})