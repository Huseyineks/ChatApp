

var notSeenMessages = document.querySelectorAll('[data-status = "not-seen"]');
window.onload = function () {

   
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
    
    
    
    
    
};




var observer = new IntersectionObserver(entries => {

    entries.forEach(entry => {

        if (entry.isIntersecting) {

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
                observer.unobserve(entry.target);
            }
            //else {

            //    notificationBox.remove();
            //    observer.disconnect();
            //    return;

            //}

            


            

                //$.ajax({

                //    type: "POST",
                //    url: "/Chat/MessageNotification",
                //    dataType: "json",
                //    data: { messageId: messageId },

                //    success: function (result) {

                //        console.log(result);
                //    },

                //    error: function (req, status, error) {

                //        console.log(status);

                //    }



                //});



           
            



        }

    });



}, {
    
    threshold : 1

});



notSeenMessages.forEach(notSeenMsg => {

    observer.observe(notSeenMsg);

});