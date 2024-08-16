var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();



//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);

    li.textContent = `${message}`;
});

connection.on("UserConnected", function (messaage) {

});

connection.on("UserDisconnected", function (message) {

});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
    alert('SignalR connection failed: ' + err.toString());
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var userId = document.getElementById("hiddenInput").value;
    var message = document.getElementById("messageInput").value;
    console.log("UserId:", userId, "Message:", message);
    connection.invoke("SendMessage",userId, message).catch(function (err) {

        return console.error(err.toString());
    });
    event.preventDefault();
});