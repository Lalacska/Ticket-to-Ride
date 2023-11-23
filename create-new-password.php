<!DOCTYPE html>
<style>
</style>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="styles/create-new-password-style.css">
    <title>New Password</title>
</head>

<body>
    <main>
        <div class="wrapper-main">

            <section class="section-default">
                <div class="back-link">
                    <a href="index.php">Go Home</a>
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