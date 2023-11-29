<!DOCTYPE html>
<!--
    This is the HTML file for the "Forgot Password" page.
    It includes a header with a navigation bar, a main section with a form for password reset,
    and a footer at the bottom of the page.
-->
<html lang="en">

<head>
    <!-- Set character set and viewport for responsive design -->
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <!-- Link to the external stylesheet for additional styling -->
    <link rel="stylesheet" href="styles/reset-password-style.css">
    <!-- Set the title of the page -->
    <title>Forgot Password</title>
</head>

<body>
    <!-- Header section containing the navigation bar -->
    <header>
        <?php
        // Include the navbar.php script for consistent navigation across pages
        require('navbar.php');
        ?>
    </header>
    <!-- Main content section -->
    <main>
        <!-- Wrapper for the main content -->
        <div class="wrapper-main">
            <!-- Section for the main content related to password reset -->
            <section class="section-default">
                <!-- Title of the page -->

                <h1 class="title">Forgot Password</h1>
                <!-- Informational paragraphs guiding the user on email input -->

                <p class="ps1">The email you have to enter is the one you</p>
                <p class="ps2">created the account with</p>
                <!-- Form for submitting email for password recovery -->

                <form action="reset-request.inc.php" method="post">
                    <!-- Label and input field for email -->

                    <label for="email">Email</label>
                    <input type="email" name="email" placeholder="Enter you e-mail address">
                    <!-- Button to submit the form -->

                    <button type="submit" name="reset-request-submit">Send Recovery Mail</button>
                </form>
                <!-- PHP code to display a success message if reset is successful -->

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
    <!-- Footer section at the bottom of the page -->
    <footer>
        <?php
        // Include the footer.php script for consistent footer across pages
        require('footer.php');
        ?>
    </footer>
</body>

</html>