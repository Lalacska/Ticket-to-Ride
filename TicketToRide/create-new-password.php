<!DOCTYPE html>
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

    .section-default {
        max-width: 1000px;
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
    }

    p {
        font-size: 12px;
        color: #ff785a;
        padding-top: 6px;
        margin-left: 170px;
    }

    label {
        font-size: 12px;
        color: #ff785a;
    }

    .ps2 {
        margin-left: 220px;
    }

    form {
        padding: 25px;
    }

    .IT {
        margin-left: 0px;
    }

    input {
        display: block;
        width: 100%;
        height: 50px;
        border-radius: 5px;
        margin-bottom: 25px;
    }

    button {
        width: 50%;
        border-radius: 5px;
        background-color: #ff785a;
        color: #fff;
    }

    a {
        font-size: 19px;
        font-weight: bold;
        color: #ff785a;
        text-decoration: none;
    }

    .back-link a:hover {
        color: #ffff;
    }
</style>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>New Password</title>
</head>

<body>
    <main>
        <div class="wrapper-main">

            <section class="section-default">
                <div class="back-link">
                    <!--Attachhing id for the javarascript to the link-->
                    <a href="#" id="executeLink">Go Home</a>
                </div>
                <h1 class="title">Create new password</h1>
                <?php
                // Check if 'selector' and 'validator' parameters are present in the URL
                $selector = isset($_GET["selector"]) ? $_GET["selector"] : '';
                $validator = isset($_GET["validator"]) ? $_GET["validator"] : '';


                // Check if either 'selector' or 'validator' is empty
                if (empty($selector) || empty($validator)) {
                    // Display error messages for missing or empty parameters
                    if (empty($selector)) {
                        echo "Missing or empty selector. ";
                    }
                    if (empty($validator)) {
                        echo "Missing or empty validator. ";
                    }
                    // Display a generic error message
                    echo "We could not validate your request.";
                } else {
                    // Check if the 'selector' is a valid hexadecimal string
                    if (ctype_xdigit($selector) !== false) {
                ?>
                        <!-- If valid, display a password reset form -->
                        <form action="reset-password.inc.php" method="post">
                            <?php
                            // Include hidden input fields with the 'selector' and 'validator' values
                            echo '<input type="hidden" name="selector" value="' . $selector . '">';
                            echo '<input type="hidden" name="validator" value="' . $validator . '">';
                            ?>
                            <!-- Input fields for the new password -->
                            <input type="password" name="pwd" placeholder="Enter a new password...">
                            <input type="password" name="pwd-repeat" placeholder="Repeat new password...">
                            <!-- Button to submit the password reset form -->
                            <button type="submit" name="reset-password-submit">Reset password</button>
                        </form>

                <?php
                    } else {
                        // Display an error message for an invalid 'selector' format
                        echo "Invalid selector format - not a valid hexadecimal string.";
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