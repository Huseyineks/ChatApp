var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();



//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);

    li.textContent = `${message}`;
});


connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    alert('SignalR connection failed: ' + err.toString());
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var receiverId = document.getElementById("hiddenReceiverInput").value;
    var authorGuid = document.getElementById("hiddenAuthorInput").value;
    var message = document.getElementById("messageInput").value;
    console.log("Message:", message);
    connection.invoke("SendMessage",authorGuid,receiverId,message).catch(function (err) {

        return console.error(err.toString());
    });
    event.preventDefault();
});