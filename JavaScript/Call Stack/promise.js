function login(){

    return new Promise(function(resolve,reject){

        console.log("Logging in...");

        setTimeout(function(){

            let user = "Rahul";

            resolve(user);

        },2000);

    });

}
function getPosts(user){

    return new Promise(function(resolve,reject){

        console.log("Getting posts for " + user);

        setTimeout(function(){

            let posts = ["Post 1", "Post 2"];

            resolve(posts);

        },2000);

    });

}
login()

.then(function(user){

    console.log("User received:", user);

    return getPosts(user);

})

.then(function(posts){

    console.log("Posts received:", posts);

});