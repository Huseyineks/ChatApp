

var notificationBox = document.getElementById("notification-box");
var profileBox = document.getElementById("profile-box");

var observer = new MutationObserver(mutations => {

    mutations.forEach(mutation => {

        if (notificationBox.textContent != 0) {

            profileBox.style.color = 'red';

            notificationBox.style.color = 'red';

        }

    });
});

observer.observe(notificationBox, { characterData: true, subtree: true });

