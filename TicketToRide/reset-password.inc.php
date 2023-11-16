<?php
// Check if the form was submitted
if (isset($_POST["reset-password-submit"])) {

    // Retrieve data from the form
    $selector = $_POST["selector"];
    $validator = $_POST["validator"];
    $password = $_POST["pwd"];
    $passwordRepeat = $_POST["pwd-repeat"];

    // Check if password fields are empty
    if (empty($password) || empty($passwordRepeat)) {
        header("Location: create-new-password.php?newpwd=empty");
        exit();
    } elseif ($password != $passwordRepeat) {
        header("Location: create-new-password.php?newpwd=pwdnotsame");
        exit();
    }

    // Get the current date and time
    $currentDate = date("U");

    // Include the connection file
    require 'connection.php';

    // Check if the reset token is valid
    $sql = "SELECT * FROM pwdReset WHERE pwdResetSelector=? AND pwdResetExpires >= ?;";
    $stmt = mysqli_stmt_init($mysqli_connection);

    if (!mysqli_stmt_prepare($stmt, $sql)) {
        echo "There was an error";
        exit();
    } else {
        mysqli_stmt_bind_param($stmt, "si", $selector, $currentDate);
        mysqli_stmt_execute($stmt);

        $result = mysqli_stmt_get_result($stmt);
        if (!$row = mysqli_fetch_assoc($result)) {
            echo "You need to re-submit your request.";
            exit();
        } else {
            $tokenBin = hex2bin($validator);
            $tokenCheck = password_verify($tokenBin, $row["pwdResetToken"]);

            if ($tokenCheck === false) {
                echo "You need to re-submit your request.";
                exit();
            } elseif ($tokenCheck === true) {

                // Token is valid, get user email
                $tokenEmail = $row['pwdResetEmail'];

                // Check if the user exists
                $sql = "SELECT * FROM sc_users WHERE email=?";
                $stmt = mysqli_stmt_init($mysqli_connection);

                if (!mysqli_stmt_prepare($stmt, $sql)) {
                    echo "Error preparing the statement: " . mysqli_stmt_error($stmt);
                    exit();
                } else {
                    mysqli_stmt_bind_param($stmt, "s", $tokenEmail);
                    mysqli_stmt_execute($stmt);

                    $result = mysqli_stmt_get_result($stmt);

                    if (!$row = mysqli_fetch_assoc($result)) {
                        echo "No user found with the provided email.";
                        exit();
                    } else {
                        // Update user password
                        $sql = "UPDATE sc_users SET password=? WHERE email=?";
                        if (!mysqli_stmt_prepare($stmt, $sql)) {
                            echo "There was an error";
                            exit();
                        } else {
                            $newPwdHash = password_hash($password, PASSWORD_DEFAULT);
                            mysqli_stmt_bind_param($stmt, "ss", $newPwdHash, $tokenEmail);
                            mysqli_stmt_execute($stmt);

                            // Delete the reset token from the database
                            $sql = "DELETE FROM pwdReset WHERE pwdResetEmail=?;";
                            if (!mysqli_stmt_prepare($stmt, $sql)) {
                                echo "There was an error";
                                exit();
                            } else {
                                mysqli_stmt_bind_param($stmt, "s", $tokenEmail);
                                mysqli_stmt_execute($stmt);
                                header("Location: login.php?newpwd=passwordupdated");
                            }
                        }
                    }
                }
            }
        }
    }
} else {
    // If the form was not submitted, redirect to the index page
    header("Location: index.php");
}
