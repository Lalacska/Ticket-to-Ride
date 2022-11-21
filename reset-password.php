<?php
?>

<style>
    * {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
    font-family: "Poppins", sans-serif;
}

body {
    height: 100vh;
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 10px;
    background: linear-gradient(135deg, #191919, #ff785a);
    width: 100%;
}

.section-default{
    max-width: 700px;
    max-height: 400px;
    height: 100%;
    width: 100%;
    background-color: #fff;
    padding: 25px 30px;
    border-radius: 5px;
    box-shadow: 0 5px 10px rgba(0, 0, 0, 0.15);
    background-color: #191919;
}

.title {
    font-size: 25px;
    font-weight: 500;
    position: relative;
    color: #ff785a;
    margin-left: 190px;
}

p{
    font-size: 12px;
    color: #ff785a;
    padding-top: 6px;
    margin-left: 170px;
}

.ps2{
    margin-left: 220px;
}

form{
    padding: 25px;
}

.IT{
    margin-left: 0px;
}

input{
    display: block;
    width: 550px;
    height: 15%;
    border-radius: 5px;
    margin-bottom: 25px;
}

button{
    width: 90%;
    height: 10%;
    margin-left: 25px;
    border-radius: 5px;
    background-color: #ff785a;
    color: #fff;
}
</style>
<main>
    <div class="wrapper-main">
        <section class="section-default">
            <h1 class="title">Forgot Password</h1>
            <p class="ps1">The email you have to enter is the one you</p>
            <p class="ps2">created the account with</p>
            <form action="reset-request.php" method="post">
                <p class="IT">Email</p>
                <input type="text" name="email" placeholder="Enter Email">
                <button type="submit" name="reset-request-submit">Send Recovery Mail</button>
            </form>
        </section>
    </div>
</main>