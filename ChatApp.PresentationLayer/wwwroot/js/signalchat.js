var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();




var button = document.getElementById("sendButton");

var messageInput = document.getElementById("messageInput");





connection.on("ReceiveMessage", function (authorGuid, message,replyingMessageId) {

        
        var receiverGuid = document.getElementById("hiddenReceiverGuid").value;

    
    if (authorGuid == receiverGuid) {

        var div = document.createElement("div");
        div.className = "msg-received";
        var msg_div = document.createElement("div");
        msg_div.className = "msg";
        document.getElementById("messagesList").appendChild(div);
        div.appendChild(msg_div);
        msg_div.textContent = `${message}`;



        div.scrollIntoView(true);

    }
    else {

        var notificationsBox = document.getElementById("notifications-box-" + authorGuid);

        


        

        if (notificationsBox == null) {

            var userBox = document.getElementById("userBox-" + authorGuid);
            var newDiv = document.createElement("div");

            newDiv.id = "notifications-box-" + authorGuid;
            newDiv.className = "not-seen-msg";

            userBox.appendChild(newDiv);

            newDiv.textContent = "1";

        }
        else {
            var currentValue = parseInt(notificationsBox.textContent);

            currentValue += 1;

            notificationsBox.textContent = currentValue;
        }
        
    }
    
});

connection.on("CallerMessage", function (message,replyingMessageId) {

    



    var p = document.createElement("p");
        var div = document.createElement("div");
        div.className = "msg-sended";
        var msg_div = document.createElement("div");
        msg_div.className = "msg";
        document.getElementById("messagesList").appendChild(div);

        div.appendChild(msg_div);
        
            

    if (replyingMessageId != null) {

        var chatRepliedBox = document.createElement("div");

        chatRepliedBox.className = "chat-replied-box";

        chatRepliedBox.classList.add("sended");

        var chatReplied = document.createElement("div");

        chatReplied.className = "chat-replied";

        var figure = document.createElement("div");

        figure.className = "figure";

        msg_div.appendChild(chatRepliedBox);

        chatRepliedBox.appendChild(figure);
        chatRepliedBox.appendChild(chatReplied);

        chatReplied.textContent = document.querySelector(".replying-to-message").textContent;

    }
    msg_div.appendChild(p);

    p.textContent = `${message}`;
    
    div.scrollIntoView(true);
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

    var replyBox = document.querySelector(".reply-box");

    var replyingToMessage = replyBox.querySelector(".replying-to-message");

    var replyingToMessageId = replyingToMessage.getAttribute("data-id");

    if (replyingToMessageId != null) {

        replyingToMessageId = parseInt(replyingToMessageId);
    }
    connection.invoke("SendMessage", authorGuid, receiverGuid, message, replyingToMessageId).catch(function (err) {

            return console.error(err.toString());
        });

   

    

    replyBox.style.display = "none";
    replyingToMessage.removeAttribute("data-id");
    messageInput.value = "";
    button.disabled = true;
    event.preventDefault();
});




