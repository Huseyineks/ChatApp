


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
           

    }
    var replyBoxes = document.querySelectorAll(".reply");
    var deleteBoxes = document.querySelectorAll(".delete");
    var forwardBoxes = document.querySelectorAll(".forward");
    var replyBoxes = document.querySelectorAll(".reply");

    replyBoxes.forEach(box => {

        box.addEventListener("click", function (event) {

            event.stopPropagation();

            var parentElement = box.parentNode.parentNode;

            var lowerBoxParentElement = document.querySelector(".lower-box-parent");

            var replyBox = lowerBoxParentElement.querySelector(".reply-box");

            var msgReplied = document.querySelector(".msg-replied");

            var message = parentElement.querySelector(".msg").textContent;

            replyBox.style.display = "block";

            var replyingToMessage = replyBox.querySelector(".replying-to-message");

            if (parentElement.classList.contains("msg-sended")) {

                

                msgReplied.style.backgroundColor = "#3797F0";

                replyBox.querySelector(".replying-to").textContent = "You're replying yourself..";

                replyingToMessage.textContent = message;

                replyingToMessage.setAttribute("data-id", parentElement.getAttribute("data-id"));
                 
            } else {

                

                    msgReplied.style.backgroundColor = "#EFEFEF";

                replyBox.querySelector(".replying-to").textContent = "You're replying to " + document.querySelector(".upper-box").querySelector(".n-name").textContent + ".."; 
                replyBox.querySelector(".replying-to-message").textContent = message;
                replyingToMessage.setAttribute("data-id", parentElement.getAttribute("data-id"));

            }
            function closeBox() {

                replyBox.style.display = "none";

                replyingToMessage.removeAttribute("data-id");

                document.querySelector(".cancel-button").removeEventListener("click", closeBox);

            }

            document.querySelector(".cancel-button").addEventListener("click", closeBox);


        });

    });



    deleteBoxes.forEach(box => {

        box.addEventListener("click", function () {

            var parentElement = box.parentNode.parentNode;

            var messageId = parentElement.getAttribute("data-id");

            var textBox = parentElement.querySelector(".msg");
            textBox.textContent = "This message is deleted.";
            

            $.ajax({

                type: "POST",
                url: "/Chat/DeleteMessage",
                dataType: "json",
                data: { messageId: messageId },

                success: function (result) {

                    console.log(result);
                },

                error: function (req, status, error) {

                    console.log(status);

                }



            });

        });

    });



    var receivedMessages = document.querySelectorAll(".msg-received");
    var sendedMessages = document.querySelectorAll(".msg-sended");
    
    receivedMessages.forEach(msg => {

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


    });
    sendedMessages.forEach(msg => {

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


    });

    var dropdown = document.querySelectorAll(".choices");
    var dropdownList = document.querySelectorAll(".dropdown");
    dropdown.forEach(list => {

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
            

             


        




    });

    
    
    
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

                $.ajax({

                    type: "POST",
                    url: "/Chat/MessageNotification",
                    dataType: "json",
                    data: { messageId: messageId },

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


document.addEventListener("DOMContentLoaded", startObserving);