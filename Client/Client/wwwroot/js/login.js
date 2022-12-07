import {LOGIN_API_URL, WEB_URL} from './config.js'

$("#account").submit(function (e) {
   let username = $("#username").val();
     let password = $("#password").val();
     e.preventDefault();
   axios({
     method: "POST",
     url: LOGIN_API_URL,
     data: {
       username: username,
       password: password,
     },
   }).then(function (response) {
       localStorage.setItem("token", response.data);
       window.location.replace(WEB_URL)
   }).catch(err => {
       $('#error').text(err.response.data.mess)
   });
 });
