<!DOCTYPE html>
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
                <?php
                $selector = isset($_GET["selector"]) ? $_GET["selector"] : '';
                $validator = isset($_GET["validator"]) ? $_GET["validator"] : '';

                if (empty($selector) || empty($validator)) {
                    if (empty($selector)) {
                        echo "Missing or empty selector. ";
                    }
                    if (empty($validator)) {
                        echo "Missing or empty validator. ";
                    }
                    echo "We could not validate your request.";
                } else {
                    if (ctype_xdigit($selector) !== false) {
                ?>

                        <form action="reset-password.inc.php" method="post">
                            <?php
                            echo '<input type="hidden" name="selector" value="' . $selector . '">';
                            echo '<input type="hidden" name="validator" value="' . $validator . '">';
                            ?>
                            <input type="password" name="pwd" placeholder="Enter a new password...">
                            <input type="password" name="pwd-repeat" placeholder="Repeat new password...">
                            <button type="submit" name="reset-password-submit">Reset password</button>
                        </form>

                <?php
                    } else {
                        echo "Invalid selector format - not a valid hexadecimal string.";
                    }
                }
                ?>
            </section>
        </div>
    </main>
</body>

</html>