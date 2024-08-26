var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();




var button = document.getElementById("sendButton");

var messageInput = document.getElementById("messageInput");





connection.on("ReceiveMessage", function (authorGuid, message) {

    
        var receiverGuid = document.getElementById("hiddenReceiverGuid").value;

    if (authorGuid == receiverGuid) {

        var div = document.createElement("div");
        div.className = "msg-received";
        var msg_div = document.createElement("div");
        msg_div.className = "msg";
        document.getElementById("messagesList").appendChild(div);
        div.appendChild(msg_div);
        msg_div.textContent = `${message}`;

    }
    else {

        var notificationsBox = document.getElementById("notifications-box-" + authorGuid);

        var currentValue = parseInt(notificationsBox.textContent);

        if (notificaitonsBox.style.display == "none") {

            notificaitonsBox.textContent = +1;
            notificaitonsBox.style.display = "block";

        }
        else {

            currentValue += 1;

            notificaitonsBox.textContent = "+" + currentValue;
        }
        
    }
    
});

connection.on("CallerMessage", function (message) {

    
        var div = document.createElement("div");
        div.className = "msg-sended";
        var msg_div = document.createElement("div");
        msg_div.className = "msg";
        document.getElementById("messagesList").appendChild(div);

        div.appendChild(msg_div);
        msg_div.textContent = `${message}`;
    
});


connection.start().then(function () {
    button.disabled = true;
    alert('SignalR connection failed: ' + err.toString());
}).catch(function (err) {
    return console.error(err.toString());
});

messageInput.addEventListener("input", function (event) {

    if (messageInput.value == "") {

        button.disabled = true;
    }
    else {
        button.disabled = false;
    }
    event.preventDefault();

});

button.addEventListener("click", function (event) {
    var receiverGuid = document.getElementById("hiddenReceiverGuid").value;
    var authorGuid = document.getElementById("hiddenAuthorGuid").value;
    
    var message = messageInput.value;
    console.log("Message:", message);
    connection.invoke("SendMessage",authorGuid,receiverGuid,message).catch(function (err) {

        return console.error(err.toString());
    });
    messageInput.value = "";
    button.disabled = true;
    event.preventDefault();
});