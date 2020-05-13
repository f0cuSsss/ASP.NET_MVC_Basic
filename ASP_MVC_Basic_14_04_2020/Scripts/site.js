document.addEventListener('DOMContentLoaded', () => {
    document.onsubmit = () => {
        // Если отправляется форма регистрации - выходим
        if (document.querySelector("input[name=RegLogin]")) {
            const login = document.getElementsByName('RegLogin')[0];
            const pass = document.getElementsByName('RegPassword')[0];
            const repeat = document.getElementsByName('RegRepeat')[0];
            const real_name = document.getElementsByName('RegRealName')[0];

            //=========================

            if (login.value === '') {
                alert("Login field should not be empty");
                return false;
            }

            if (login.value.length < 3) {
                alert("Login less then 3 symbols");
                return false;
            }

            //=========================

            if (pass.value === '') {
                alert("Password field should not be empty");
                return false;
            }

            if (repeat.value === '') {
                alert("Repeat password field should not be empty");
                return false;
            }

            if (pass.value.length < 6) {
                alert("Password less then 6 symbols");
                return false;
            }

            if (pass.value != repeat.value) {
                alert("Passwords do not match");
                return false;
            }

            //=========================


            return true;
        }

    
        // Форма из RazorDemo
        if (window.Name) return true;

        // Форма из Book/Genre
        if (window.genreAddButton) return true;

        // Форма из Book/City
        if (window.cityAddButton) return true;

        //if (document.querySelector("input[name=UserLogin]")) {
            const user = document.getElementsByName('UserLogin')[0];
            const pass = document.getElementsByName('UserPassword')[0];

            if (user.value === '') {
                alert("Login field should not be empty");
                return false;
            }

            if (user.value.length < 3) {
                alert("Login less then 3 symbols");
                return false;
            }

            if (pass.value === '') {
                alert("Password field should not be empty");
                return false;
            }

            if (pass.value.length < 6) {
                alert("Password less then 6 symbols");
                return false;
            }

            //var r = /[^A-Z-a-z-0-9]/g; 
            //if (!r.test(user) === false) {
            //    alert("Login must contain only latin letters");
            //    return false;
            //}


            return true;
        //}
    }
});

