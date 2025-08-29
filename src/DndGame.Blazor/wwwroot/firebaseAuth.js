import { getAuth, signInWithEmailAndPassword } from "https://www.gstatic.com/firebasejs/12.2.1/firebase-auth.js";

window.firebaseAuth = {
    signIn: function (email, password) {
        const auth = getAuth();
        return signInWithEmailAndPassword(auth, email, password)
            .then(userCredential => userCredential.user.uid)
            .catch(err => {
                console.error(err);
                return "";
            });
    },
    getCurrentUser: function () {
        const auth = getAuth();
        const user = auth.currentUser;
        return user ? user.uid : "";
    }
};
