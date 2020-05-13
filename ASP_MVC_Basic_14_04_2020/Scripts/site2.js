document.addEventListener('DOMContentLoaded', () => {
    var addAuthor = document.getElementById("addAuthorButton");
    if (addAuthor) {
        addAuthor.addEventListener("click", addAuthorClick);
    } else {
        console.log("addAuthor button not found");
    }

    loadAuthorList();
});



function addAuthorClick() {
    var addBlock = document.getElementById("addAuthorBlock");
    if (!addBlock) { console.error("addAuthorBlock not found"); return }

    var nameInput = addBlock.querySelector("input[name=Name]");
    if (!nameInput) { console.error("nameInput not found"); return }

    var pseudoInput = addBlock.querySelector("input[name=Pseudo]");
    if (!pseudoInput) { console.error("pseudoInput not found"); return }

    var authorName = nameInput.value.trim();
    var authorPseudo = pseudoInput.value.trim();


    if (authorName.length == 0) {
        alert("Name should not be empty");
        return;
    }

    var x = new XMLHttpRequest();

    x.onreadystatechange = function () {
        if (x.readyState == 4) {
            //alert(x.responseText);
            var ans = JSON.parse(x.responseText);
            if (typeof ans.status == 'undefined') { alert('Invalid server answer'); return; }

            switch (ans.status) {
                case 1: alert('Invalid data sent'); return;
                case 2: alert('Author in DB'); return;
                case 3: alert(ans.msg); return;
                case 0:
                    alert('Add OK');
                    nameInput.value = "";
                    pseudoInput.value = "";


                    //var g = document.getElementById("ul_authors");
                    //var li = document.createElement("li");
                    //li.innerHTML = "<b>" + authorName + "</b> (<i>" + authorPseudo + "</i>)";
                    //g.appendChild(li);


                    return;
            }
        }
    }

    x.open("POST", "/Book/AddAuthor", true);
    x.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
    x.send("Name=" + authorName + "&Pseudo=" + authorPseudo);

    
}

function loadAuthorList() {
    var container = document.getElementById("authorList");

    if (!container) {
        console.log("Container location error: authorList");
        return;
    }

    var x = new XMLHttpRequest();

    x.onreadystatechange = function () {
        if (x.readyState == 4) {
            if (x.status == 200) {
                var data = JSON.parse(x.responseText);
                var txt = "<ul class=\"list-group list-group-flush\">";

                for (let author of data) {
                    let pseudo = author.Pseudo == "" ? " " : author.Pseudo;
                    txt += "<li class=\"list-group-item\" name=" + author.Id + " ><b onclick='authorNameClick(event)'>" + author.Name + "</b> (<i onclick='authorPseudoClick(event)'>" + pseudo + "</i>)</li>";
                }

                txt += "<ul>";
                container.innerHTML = txt;
            }
            else
            {
                container.innerText = "Error occured while data loaded. Reload page, please.";
                console.log(x.responseText);
            }
        }
    }
    x.open("GET", "/Book/AuthorList", true);
    x.send();
}

function authorNameClick(e) {
    var b = e.target;
    var i = document.createElement('input');

    i.style.width = b.offsetWidth + 10 + 'px';

    i.onblur = authorNameBlur;
    b.style.display = 'none';
    i.value = b.innerText;

    b.parentNode.insertBefore(i, b);
}

function authorNameBlur(e) {
    var i = e.target;
    var b = i.parentNode.querySelector('B');

    b.innerText = i.value;
    b.style.display = 'inline';
    i.parentNode.removeChild(i);

    var authorId = b.parentNode.attributes["name"].value;
    var newName = b.innerText;

    //alert("New name: " + newName + "; id: " + authorId);


    var x = new XMLHttpRequest();
    x.onreadystatechange = function () {
        if (x.readyState == 4) {
            var res = JSON.parse(x.responseText);
            if (res.status != 0)
                alert(res.message); // Ошибка при обработке на сервере
            loadAuthorList();   // Успешная обработка - обновляем список
        }
    }
    x.open("POST", "/Book/EditAuthor", true);
    x.setRequestHeader("Content-Type", "application/json");
    //x.setRequestHeader("Content-Type", "application/x-www-form-urlencoded"); // id=1&name=sometext
    x.send('{"Id":' + authorId + ', "Name":"' + newName + '"}');


}


function authorPseudoClick(e) {
    var i = e.target;
    var input = document.createElement('input');

    input.style.width = i.offsetWidth + 10 + 'px';

    input.onblur = authorPseudoBlur;
    i.style.display = 'none';
    input.value = i.innerText;

    i.parentNode.insertBefore(input, i);
}

function authorPseudoBlur(e) {
    var target = e.target;
    var i = target.parentNode.querySelector('i');

    i.innerText = target.value;
    i.style.display = 'inline';
    target.parentNode.removeChild(target);

    var authorId = i.parentNode.attributes["name"].value;
    var newPseudo = i.innerText;

    var x = new XMLHttpRequest();
    x.onreadystatechange = function () {
        if (x.readyState == 4) {
            
            var res = JSON.parse(x.responseText);
            if (res.status == 0)
                loadAuthorList();
            
        }
    }
    x.open("POST", "/Book/EditAuthorPseudo", true);
    x.setRequestHeader("Content-Type", "application/json");
    x.send('{"Id":' + authorId + ', "Pseudo":"' + newPseudo + '"}');
}