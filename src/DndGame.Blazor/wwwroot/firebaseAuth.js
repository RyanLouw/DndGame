window.firebaseAuth = {
    init: function (config) {
        firebase.initializeApp(config);
    },
    signIn: function (email, password) {
        return firebase.auth().signInWithEmailAndPassword(email, password)
            .then(userCredential => userCredential.user.uid)
            .catch(err => {
                console.error(err);
                return "";
            });
    }
};
