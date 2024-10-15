var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();




var button = document.getElementById("sendButton");

var messageInput = document.getElementById("messageInput");





connection.on("ReceiveMessage", function (authorGuid, message,replyingMessage,messageId,replyingTo,repliedMessageId) {

        
        var receiverGuid = document.getElementById("hiddenReceiverGuid").value;

    
    if (authorGuid == receiverGuid) {


        var p = document.createElement("p");
        var div = document.createElement("div");
        div.className = "msg-received";

        div.setAttribute("data-id", messageId);

        div.innerHTML += ` <div class="dropdown">
                                                <div class="forward">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-90deg-right" viewBox="0 0 16 16">
                                                        <path fill-rule="evenodd" d="M14.854 4.854a.5.5 0 0 0 0-.708l-4-4a.5.5 0 0 0-.708.708L13.293 4H3.5A2.5 2.5 0 0 0 1 6.5v8a.5.5 0 0 0 1 0v-8A1.5 1.5 0 0 1 3.5 5h9.793l-3.147 3.146a.5.5 0 0 0 .708.708z" />
                                                    </svg>Forward
                                                </div>

                                                <div class="reply">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-90deg-left" viewBox="0 0 16 16">
                                                        <path fill-rule="evenodd" d="M1.146 4.854a.5.5 0 0 1 0-.708l4-4a.5.5 0 1 1 .708.708L2.707 4H12.5A2.5 2.5 0 0 1 15 6.5v8a.5.5 0 0 1-1 0v-8A1.5 1.5 0 0 0 12.5 5H2.707l3.147 3.146a.5.5 0 1 1-.708.708z" />
                                                    </svg>Reply
                                                </div>

                                                <div class="delete">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash3" viewBox="0 0 16 16">
                                                        <path d="M6.5 1h3a.5.5 0 0 1 .5.5v1H6v-1a.5.5 0 0 1 .5-.5M11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3A1.5 1.5 0 0 0 5 1.5v1H1.5a.5.5 0 0 0 0 1h.538l.853 10.66A2 2 0 0 0 4.885 16h6.23a2 2 0 0 0 1.994-1.84l.853-10.66h.538a.5.5 0 0 0 0-1zm1.958 1-.846 10.58a1 1 0 0 1-.997.92h-6.23a1 1 0 0 1-.997-.92L3.042 3.5zm-7.487 1a.5.5 0 0 1 .528.47l.5 8.5a.5.5 0 0 1-.998.06L5 5.03a.5.5 0 0 1 .47-.53Zm5.058 0a.5.5 0 0 1 .47.53l-.5 8.5a.5.5 0 1 1-.998-.06l.5-8.5a.5.5 0 0 1 .528-.47M8 4.5a.5.5 0 0 1 .5.5v8.5a.5.5 0 0 1-1 0V5a.5.5 0 0 1 .5-.5" />
                                                    </svg>Delete
                                                </div>


                                            </div>
                                            <div class="choices">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-caret-down" viewBox="0 0 16 16">
                                                    <path d="M3.204 5h9.592L8 10.481zm-.753.659 4.796 5.48a1 1 0 0 0 1.506 0l4.796-5.48c.566-.647.106-1.659-.753-1.659H3.204a1 1 0 0 0-.753 1.659" />
                                                </svg>
                                            </div>`;
        var msg_div = document.createElement("div");
        msg_div.className = "msg";
        document.getElementById("messagesList").appendChild(div);
        div.appendChild(msg_div);
        if (replyingMessage != null) {

            var chatRepliedBox = document.createElement("div");

            chatRepliedBox.className = "chat-replied-box";

            chatRepliedBox.classList.add(replyingTo == "self" ? "received" : "sended");

            chatRepliedBox.setAttribute("data-id", repliedMessageId);

            var chatReplied = document.createElement("div");

            chatReplied.className = "chat-replied";

            var figure = document.createElement("div");

            figure.className = "figure";

            msg_div.appendChild(chatRepliedBox);

            chatRepliedBox.appendChild(figure);
            chatRepliedBox.appendChild(chatReplied);

            chatReplied.textContent = replyingMessage;

            repliedMessageEventListener(chatRepliedBox);

        }
        msg_div.appendChild(p);

        p.textContent = `${message}`;


        replyBoxEventListener(div.querySelector(".reply"));

        deleteBoxEventListener(div.querySelector(".delete"));

        forwardBoxEventListener(div.querySelector(".forward"));

        receivedMessageEventListener(div);

        dropdownEventListener(div.querySelector(".choices"));

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

connection.on("GroupMessage", function (message, receiverGuid, messageId, replyingMessage ,replyingTo, repliedMessageId) {

   
    var guid = document.getElementById("hiddenReceiverGuid").value;

    if (receiverGuid == guid) {


        var p = document.createElement("p");
        var div = document.createElement("div");
        div.className = "msg-received";

        div.setAttribute("data-id", messageId);

        div.innerHTML += ` <div class="dropdown">
                                                <div class="forward">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-90deg-right" viewBox="0 0 16 16">
                                                        <path fill-rule="evenodd" d="M14.854 4.854a.5.5 0 0 0 0-.708l-4-4a.5.5 0 0 0-.708.708L13.293 4H3.5A2.5 2.5 0 0 0 1 6.5v8a.5.5 0 0 0 1 0v-8A1.5 1.5 0 0 1 3.5 5h9.793l-3.147 3.146a.5.5 0 0 0 .708.708z" />
                                                    </svg>Forward
                                                </div>

                                                <div class="reply">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-90deg-left" viewBox="0 0 16 16">
                                                        <path fill-rule="evenodd" d="M1.146 4.854a.5.5 0 0 1 0-.708l4-4a.5.5 0 1 1 .708.708L2.707 4H12.5A2.5 2.5 0 0 1 15 6.5v8a.5.5 0 0 1-1 0v-8A1.5 1.5 0 0 0 12.5 5H2.707l3.147 3.146a.5.5 0 1 1-.708.708z" />
                                                    </svg>Reply
                                                </div>

                                                <div class="delete">
                                                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash3" viewBox="0 0 16 16">
                                                        <path d="M6.5 1h3a.5.5 0 0 1 .5.5v1H6v-1a.5.5 0 0 1 .5-.5M11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3A1.5 1.5 0 0 0 5 1.5v1H1.5a.5.5 0 0 0 0 1h.538l.853 10.66A2 2 0 0 0 4.885 16h6.23a2 2 0 0 0 1.994-1.84l.853-10.66h.538a.5.5 0 0 0 0-1zm1.958 1-.846 10.58a1 1 0 0 1-.997.92h-6.23a1 1 0 0 1-.997-.92L3.042 3.5zm-7.487 1a.5.5 0 0 1 .528.47l.5 8.5a.5.5 0 0 1-.998.06L5 5.03a.5.5 0 0 1 .47-.53Zm5.058 0a.5.5 0 0 1 .47.53l-.5 8.5a.5.5 0 1 1-.998-.06l.5-8.5a.5.5 0 0 1 .528-.47M8 4.5a.5.5 0 0 1 .5.5v8.5a.5.5 0 0 1-1 0V5a.5.5 0 0 1 .5-.5" />
                                                    </svg>Delete
                                                </div>


                                            </div>
                                            <div class="choices">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-caret-down" viewBox="0 0 16 16">
                                                    <path d="M3.204 5h9.592L8 10.481zm-.753.659 4.796 5.48a1 1 0 0 0 1.506 0l4.796-5.48c.566-.647.106-1.659-.753-1.659H3.204a1 1 0 0 0-.753 1.659" />
                                                </svg>
                                            </div>`;
        var msg_div = document.createElement("div");
        msg_div.className = "msg";
        document.getElementById("messagesList").appendChild(div);
        div.appendChild(msg_div);

        if (replyingMessage != null) {

            var chatRepliedBox = document.createElement("div");

            chatRepliedBox.className = "chat-replied-box";

            chatRepliedBox.classList.add(replyingTo == "self" ? "received" : "sended");

            chatRepliedBox.setAttribute("data-id", repliedMessageId);

            var chatReplied = document.createElement("div");

            chatReplied.className = "chat-replied";

            var figure = document.createElement("div");

            figure.className = "figure";

            msg_div.appendChild(chatRepliedBox);

            chatRepliedBox.appendChild(figure);
            chatRepliedBox.appendChild(chatReplied);

            chatReplied.textContent = replyingMessage;

            repliedMessageEventListener(chatRepliedBox);

        }

        msg_div.appendChild(p);
        p.textContent = `${message}`;

        replyBoxEventListener(div.querySelector(".reply"));

        deleteBoxEventListener(div.querySelector(".delete"));

        forwardBoxEventListener(div.querySelector(".forward"));

        receivedMessageEventListener(div);

        dropdownEventListener(div.querySelector(".choices"));


        div.scrollIntoView(true);


    }
    else {


        var notificationsBox = document.getElementById("notifications-box-" + receiverGuid);






        if (notificationsBox == null) {

            var userBox = document.getElementById("userBox-" + receiverGuid);
            var newDiv = document.createElement("div");

            newDiv.id = "notifications-box-" + receiverGuid;
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

connection.on("CallerMessage", function (message,replyingMessage,messageId,repliedMessageId) {

    



    var p = document.createElement("p");
    var div = document.createElement("div");
    div.className = "msg-sended";

    div.setAttribute("data-id", messageId);
    var msg_div = document.createElement("div");
    msg_div.className = "msg";
    document.getElementById("messagesList").appendChild(div);

    div.appendChild(msg_div);

    

    if (replyingMessage != null) {

        var chatRepliedBox = document.createElement("div");

        chatRepliedBox.className = "chat-replied-box";

        chatRepliedBox.setAttribute("data-id", repliedMessageId);

        chatRepliedBox.classList.add(document.querySelector(".replying-to-message").getAttribute("data-author") == "self" ? "sended" : "received");

        var chatReplied = document.createElement("div");

        chatReplied.className = "chat-replied";

        var figure = document.createElement("div");

        figure.className = "figure";
        
        msg_div.appendChild(chatRepliedBox);

        chatRepliedBox.appendChild(figure);
        chatRepliedBox.appendChild(chatReplied);

        chatReplied.textContent = replyingMessage;

       

    }
    msg_div.appendChild(p);

    p.textContent = `${message}`;

    div.innerHTML += ` <div class="choices">
                                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-caret-down" viewBox="0 0 16 16">
                                                <path d="M3.204 5h9.592L8 10.481zm-.753.659 4.796 5.48a1 1 0 0 0 1.506 0l4.796-5.48c.566-.647.106-1.659-.753-1.659H3.204a1 1 0 0 0-.753 1.659" />
                                            </svg>
                                        </div>
                                        <div class="dropdown">
                                            <div class="forward">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-90deg-right" viewBox="0 0 16 16">
                                                    <path fill-rule="evenodd" d="M14.854 4.854a.5.5 0 0 0 0-.708l-4-4a.5.5 0 0 0-.708.708L13.293 4H3.5A2.5 2.5 0 0 0 1 6.5v8a.5.5 0 0 0 1 0v-8A1.5 1.5 0 0 1 3.5 5h9.793l-3.147 3.146a.5.5 0 0 0 .708.708z" />
                                                </svg>Forward
                                            </div>

                                            <div class="reply">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-arrow-90deg-left" viewBox="0 0 16 16">
                                                    <path fill-rule="evenodd" d="M1.146 4.854a.5.5 0 0 1 0-.708l4-4a.5.5 0 1 1 .708.708L2.707 4H12.5A2.5 2.5 0 0 1 15 6.5v8a.5.5 0 0 1-1 0v-8A1.5 1.5 0 0 0 12.5 5H2.707l3.147 3.146a.5.5 0 1 1-.708.708z" />
                                                </svg>Reply
                                            </div>

                                            <div class="delete">
                                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash3" viewBox="0 0 16 16">
                                                    <path d="M6.5 1h3a.5.5 0 0 1 .5.5v1H6v-1a.5.5 0 0 1 .5-.5M11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3A1.5 1.5 0 0 0 5 1.5v1H1.5a.5.5 0 0 0 0 1h.538l.853 10.66A2 2 0 0 0 4.885 16h6.23a2 2 0 0 0 1.994-1.84l.853-10.66h.538a.5.5 0 0 0 0-1zm1.958 1-.846 10.58a1 1 0 0 1-.997.92h-6.23a1 1 0 0 1-.997-.92L3.042 3.5zm-7.487 1a.5.5 0 0 1 .528.47l.5 8.5a.5.5 0 0 1-.998.06L5 5.03a.5.5 0 0 1 .47-.53Zm5.058 0a.5.5 0 0 1 .47.53l-.5 8.5a.5.5 0 1 1-.998-.06l.5-8.5a.5.5 0 0 1 .528-.47M8 4.5a.5.5 0 0 1 .5.5v8.5a.5.5 0 0 1-1 0V5a.5.5 0 0 1 .5-.5" />
                                                </svg>Delete
                                            </div>


                                        </div>`;  


    sendedMessageEventListener(div);

    replyBoxEventListener(div.querySelector(".reply"));

    deleteBoxEventListener(div.querySelector(".delete"));

    forwardBoxEventListener(div.querySelector(".forward"));

    dropdownEventListener(div.querySelector(".choices"));

    if (div.querySelector(".chat-replied-box") != null) {

        repliedMessageEventListener(div.querySelector(".chat-replied-box"));

    }
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
    
    
    var message = messageInput.value;
    console.log("Message:", message);

    var replyBox = document.querySelector(".reply-box");

    var replyingToMessage = replyBox.querySelector(".replying-to-message");

    var replyingToMessageId = replyingToMessage.getAttribute("data-id");

    if (replyingToMessageId != null) {

        replyingToMessageId = parseInt(replyingToMessageId);

    }

    var type = document.getElementById("GroupOrPrivate");

    if (type.value == "Private") {
        connection.invoke("SendMessage", receiverGuid, message, replyingToMessageId).catch(function (err) {

            return console.error(err.toString());
        });

    } else if(type.value == "Group") {

        connection.invoke("SendMessageToGroup", message, receiverGuid, replyingToMessageId).catch(function (err) {

            return console.error(err.toString());
        });

    }

   

    

    replyBox.style.display = "none";
    replyingToMessage.removeAttribute("data-id");
    messageInput.value = "";
    button.disabled = true;
    
    event.preventDefault();
});




