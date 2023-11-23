<!DOCTYPE html>
<style>
    
</style>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="styles/reset-password-style.css">
    <title>Forgot Password</title>
</head>

<body>
    <main>
        <div class="wrapper-main">
            <section class="section-default">
                <h1 class="title">Forgot Password</h1>
                <p class="ps1">The email you have to enter is the one you</p>
                <p class="ps2">created the account with</p>
                <form action="reset-request.inc.php" method="post">
                    <label for="email">Email</label>
                    <input type="email" name="email" placeholder="Enter you e-mail address">
                    <button type="submit" name="reset-request-submit">Send Recovery Mail</button>
                    <div class="back-link">
                        <!--Attachhing id for the javarascript to the link-->
                        <a href="index.php">Go Home</a>
                    </div>
                </form>
                <?php
                // Check if the "reset" parameter is set in the URL
                if (isset($_GET["reset"])) {
                    // Check if the value of the "reset" parameter is "success"
                    if ($_GET["reset"] == "success") {
                        // If true, display a success message in a paragraph with the class "signupsuccess"
                        echo '<p class="signupsuccess"> Check your e-mail!</p>';
                    }
                }
                ?>
            </section>
        </div>
    </main>
    <!-- Include jQuery library -->
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <!-- link to our javascript file for the go home button -->
    <script src="javascripts/script.js"></script>
</body>

</html>