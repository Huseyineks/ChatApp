


window.onload = function () {

    var notSeenMessages = document.querySelectorAll('[data-status = "not-seen"]');   
    var topDiv = null;
    var topPosition = Infinity;
    if (notSeenMessages.length > 0) {



        notSeenMessages.forEach(notSeenMessage => {

            const position = notSeenMessage.getBoundingClientRect();

            if (position.top < topPosition) {

                topDiv = notSeenMessage;

                topPosition = position.top;


            }


        });






        topDiv.scrollIntoView({ behavior: "smooth", block: "start", inline: "nearest" });


    } else {


        var container = document.getElementById("pastMessages");

        container.lastElementChild.scrollIntoView({ behavior: "instant", block: "start", inline: "nearest" });


    }
   

    startEventListener();
    
    
};




var observer = new IntersectionObserver(entries => {

    entries.forEach(entry => {
        console.log("Observing element:", entry.target.textContent);

        if (entry.isIntersecting) {
            console.log("Element intersecting:", entry.target.textContent);
            entry.target.setAttribute("data-status", "seen");
            var receiverGuid = document.getElementById("hiddenReceiverGuid").value;

            var notificationBox = document.getElementById("notifications-box-" + receiverGuid);

            var currentValue = parseInt(notificationBox.textContent);

            

            currentValue -= 1;

            if (currentValue => 0) {


                

                var messageId = entry.target.getAttribute("data-id");

                var messageType;

                if (messageId.includes('G')) {

                    messageId = messageId.split('-')[1];

                    messageType = "Group";
                } else {

                    messageType = "Private";
                }

                $.ajax({

                    type: "POST",
                    url: "/Chat/MessageNotification",
                    dataType: "json",
                    data: { messageId: messageId, messageType : messageType },

                    success: function (result) {

                        console.log(result);
                    },

                    error: function (req, status, error) {

                        console.log(status);

                    }



                });
                if (currentValue == 0) {

                    notificationBox.remove();
                    observer.disconnect();
                    return;

                }
                notificationBox.textContent = currentValue;
                console.log("Unobserving element:", entry.target.textContent);
                observer.unobserve(entry.target);
            }
         



           
            



        }

    });



}, {
    
    threshold : 1

});

function startObserving() {
    var notSeenMessages = document.querySelectorAll('[data-status="not-seen"]');
    notSeenMessages.forEach(notSeenMsg => {
        console.log("Starting observation for element:", notSeenMsg.textContent); 
        observer.observe(notSeenMsg);
    });
}

function replyBoxEventListener(box) {

    box.addEventListener("click", function (event) {

        event.stopPropagation();
        var parentElement = box.parentNode.parentNode;

        var lowerBoxParentElement = document.querySelector(".lower-box-parent");

        var replyBox = lowerBoxParentElement.querySelector(".reply-box");

        var msgReplied = document.querySelector(".msg-replied");

        var message = parentElement.querySelector(".msg p").textContent;

        replyBox.style.display = "block";

        var replyingToMessage = replyBox.querySelector(".replying-to-message");

        if (parentElement.classList.contains("msg-sended")) {



            msgReplied.style.backgroundColor = "#e27689";

            replyBox.querySelector(".replying-to").textContent = "You're replying yourself..";

            replyingToMessage.textContent = message;

            replyingToMessage.setAttribute("data-id", parentElement.getAttribute("data-id"));

            replyingToMessage.setAttribute("data-author", "self");
        } else {



            msgReplied.style.backgroundColor = "#EFEFEF";

            replyBox.querySelector(".replying-to").textContent = "You're replying to " + document.querySelector(".upper-box").querySelector(".n-name").textContent + "..";
            replyBox.querySelector(".replying-to-message").textContent = message;
            replyingToMessage.setAttribute("data-id", parentElement.getAttribute("data-id"));

            replyingToMessage.setAttribute("data-author", "other");

        }
        function closeBox() {

            replyBox.style.display = "none";

            replyingToMessage.removeAttribute("data-id");

            document.querySelector(".cancel-button").removeEventListener("click", closeBox);

        }

        document.querySelector(".cancel-button").addEventListener("click", closeBox);

    });
}



function deleteBoxEventListener(box) {

    box.addEventListener("click", function () {

        var parentElement = box.parentNode.parentNode;

        var messageId = parentElement.getAttribute("data-id");

        var textBox = parentElement.querySelector(".msg");
        textBox.textContent = "This message is deleted.";

        if (messageId.includes("G")) {

            messageId = messageId.split('-')[1];

            messageType = "Group";
        } else {

            messageType = "Private";
        }

        var receiverGuid = document.getElementById("hiddenReceiverGuid").value;

        $.ajax({

            type: "POST",
            url: "/Chat/DeleteMessage",
            dataType: "json",
            data: { messageId: messageId, messageType : messageType, receiverGuid : receiverGuid },

            success: function (result) {

                console.log(result);
            },

            error: function (req, status, error) {

                console.log(status);

            }



        });

    });

}

function receivedMessageEventListener(msg) {

    msg.addEventListener("mouseover", function (event) {

        var choiceBox = msg.querySelector(".choices");

        choiceBox.style.display = "flex";


        event.preventDefault();
    });

    msg.addEventListener("mouseout", function (event) {


        var choiceBox = msg.querySelector(".choices");

        choiceBox.style.display = "none";


        event.preventDefault();

    });

}

function sendedMessageEventListener(msg) {

    msg.addEventListener("mouseover", function (event) {

        var choiceBox = msg.querySelector(".choices");

        choiceBox.style.display = "flex";


        event.preventDefault();
    });

    msg.addEventListener("mouseout", function (event) {


        var choiceBox = msg.querySelector(".choices");

        choiceBox.style.display = "none";


        event.preventDefault();

    });

}

function dropdownEventListener(list) {


    var dropdownList = document.querySelectorAll(".dropdown");
    list.addEventListener("click", function (event) {

        dropdownList.forEach(element => {
            element.style.display = "none";
        });

        event.stopPropagation();


        var parentElement = list.parentNode;

        var dropdownElement = parentElement.querySelector(".dropdown");

        dropdownElement.style.display = "block";


        function clickOnOutside() {

            if (!dropdownElement.contains(event.target)) {

                dropdownElement.style.display = "none";

                document.removeEventListener("click", clickOnOutside);
            }


        }

        document.addEventListener("click", clickOnOutside);


    });


}

function repliedMessageEventListener(box) {   

    box.addEventListener("click", function (event) {


        var messageId = box.getAttribute("data-id");

        var message = document.querySelector(`.msg-received[data-id="${messageId}"], .msg-sended[data-id="${messageId}"]`);

        message.scrollIntoView({ behavior: "smooth", block: "start", inline: "nearest" });


    });


}

function forwardBoxEventListener(box) {

    var modal = document.getElementById("forwardMessageModal");

    var modalContent = modal.querySelector(".modal-content");
    var modalButton = document.getElementById("forwardMessageButton");
    var forwaredMessage;
    box.addEventListener("click", function (event) {

        
        modal.style.display = "flex";

        forwaredMessage = box.parentNode.parentNode.querySelector("p").textContent;

        modalButton.value = forwaredMessage;


    });

    modal.addEventListener("click", (event) => {


        if (!modalContent.contains(event.target)) {


            modal.style.display = "none";

            var checkBoxes = document.querySelectorAll("#checkBoxButton");

            checkBoxes.forEach(box => {

                box.checked = false;
                

            });

            


            

            modalButton.disabled = true;
        }




    });


}

function checkBoxEventListener(box) {

    

    

    var allBoxes = document.querySelectorAll("#checkBoxButton");
    box.addEventListener("change", function (event) {

        var modalButton = box.parentNode.parentNode.parentNode.parentNode.querySelector(".modal-button");

        if (box.checked) {

            modalButton.disabled = false;


        }

        else {

            if (areAllUnchecked(allBoxes)) {

                modalButton.disabled = true;
            }

        }

          


    });




}


function areAllUnchecked(checkBoxes) {
    return [...checkBoxes].every(checkBox => !checkBox.checked);
}


function createGroup() {

    var modal = document.getElementById("createGroupModal");

    modal.style.display = "flex";

    var modalContent = modal.querySelector(".modal-content");


    modal.addEventListener("click", function (event) {


        if (!modalContent.contains(event.target)) {


            modal.style.display = "none";



            var checkBoxes = document.querySelectorAll("#checkBoxButton");

            checkBoxes.forEach(box => {

                box.checked = false;


            });


            var groupNameInput = document.getElementById("groupName");

            var groupImageInput = document.getElementById("groupImage");


           


            groupImageInput.value = '';
            groupNameInput.value = '';



            modalButton.disabled = true;


        }





    });



}



function startEventListener() {

    var replyBoxes = document.querySelectorAll(".reply");
    var deleteBoxes = document.querySelectorAll(".delete");
    var forwardBoxes = document.querySelectorAll(".forward");
    var replyBoxes = document.querySelectorAll(".reply");
    var repliedMessagesBoxes = document.querySelectorAll(".chat-replied-box");

    var checkBoxes = document.querySelectorAll("#checkBoxButton");

    var forwardMessageButton = document.getElementById("forwardMessageButton");

    var createGroupButton = document.getElementById("createGroupButton");


    createGroupButton.addEventListener("click", function (event) {

        var groupNameInput = document.getElementById("groupName");

        var groupImageInput = document.getElementById("groupImage");

        var files = groupImageInput.files;


        var file = files[0];


       
        

        
        var checkBoxes = createGroupButton.parentNode.querySelectorAll("#checkBoxButton");

        let usersGuid = new Array();

        checkBoxes.forEach(box => {

            if (box.checked) {

                usersGuid.push(box.value);

                box.checked = false;

            }

        });
        document.getElementById("createGroupModal").style.display = "none";

        createGroupButton.disabled = true;

        // rresim sonra 


        $.ajax({

            type: "POST",
            url: "/Chat/CreateGroup",
            dataType: "json",
            data: { usersGuid: usersGuid, groupName: groupNameInput.value, groupImage: null },

            success: function (result) {

                console.log(result);
            },

            error: function (req, status, error) {

                alert(error);

            }



        });

        groupImageInput.value = '';
        groupNameInput.value = '';
    });


    forwardMessageButton.addEventListener("click", function (event) {

       


        var checkBoxes = forwardMessageButton.parentNode.querySelectorAll("#checkBoxButton");

        let usersGuid = new Array();

        checkBoxes.forEach(box => {

            if (box.checked) {

                usersGuid.push(box.value);

                box.checked = false;

            }

        });
        document.getElementById("forwardMessageModal").style.display = "none";

        forwardMessageButton.disabled = true;

        

        $.ajax({

            type: "POST",
            url: "/Chat/ForwardMessage",
            dataType: "json",
            data: { usersGuid: usersGuid, message: forwardMessageButton.value },

            success: function (result) {

                console.log(result);
            },

            error: function (req, status, error) {

                console.log(status);

            }



        });





    });

    


    checkBoxes.forEach(box => {


        checkBoxEventListener(box);


    });

    forwardBoxes.forEach(box => {


        forwardBoxEventListener(box);


    });

    repliedMessagesBoxes.forEach(box => {

        repliedMessageEventListener(box);

    });

    replyBoxes.forEach(box => {

        replyBoxEventListener(box);

    });



    deleteBoxes.forEach(box => {

        deleteBoxEventListener(box);

    });



    var receivedMessages = document.querySelectorAll(".msg-received");
    var sendedMessages = document.querySelectorAll(".msg-sended");

    receivedMessages.forEach(msg => {

        receivedMessageEventListener(msg);


    });
    sendedMessages.forEach(msg => {

        sendedMessageEventListener(msg);


    });

    var dropdown = document.querySelectorAll(".choices");
    
    dropdown.forEach(list => {

       



        dropdownEventListener(list);





    });


}


//document.addEventListener("DOMContentLoaded", startObserving);

document.addEventListener("DOMContentLoaded", function () {

    startObserving();

    var successToastEl = document.getElementById('successToast');
    if (successToastEl) {
        var successToast = new bootstrap.Toast(successToastEl);
        successToast.show();
    }


    var errorToastEl = document.getElementById('errorToast');
    if (errorToastEl) {
        var errorToast = new bootstrap.Toast(errorToastEl);
        errorToast.show();
    }
});