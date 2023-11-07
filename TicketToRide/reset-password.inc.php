<?php

if (isset($_POST["reset-password-submit"])) {

    // bind global variables.
    $selector = $_POST["selector"];
    $validator = $_POST["validator"];
    $password = $_POST["pwd"];
    $passwordRepeat = $_POST["pwd-repeat"];

    //If empty will redirect you.
    if (empty($password) || empty($passwordRepeat)) {
        header("location: /create-new-password.php?newpwd=empty");
        exit();
    } else if ($password != $passwordRepeat) {
        header("location: /create-new-password.php?newpwd=pwdnotsame");
        exit();
    }

    //Current date.
    $currentDate = date("U");

    require 'connection.php';

    //sql statements where you get everything from pwdReset where pwdResetSelector is something and pwdResetExpire is bigger or equal to something.
    $sql = "SELECT * FROM pwdReset WHERE pwdResetSelector=? AND pwdResetExpire >= ?";
    $stmt = mysqli_stmt_init($mysqli_connection);
    if (!mysqli_stmt_prepare($stmt, $sql)) {
        echo "There was an error! OG";
        exit();
    } else {

        //We bind variabels to a prepared statement.
        mysqli_stmt_bind_param($stmt, "ss", $selector, $currentDate);

        //Execute statement.
        mysqli_stmt_execute($stmt);

        $result = mysqli_stmt_get_result($stmt);
        //fetch data from $result and returns it as an asociative array.
        if (!$row = mysqli_fetch_assoc($result)) {
            echo "You need to re-submit your request ";
            exit();
        } else {

            //Convert validator token to binary.
            $tokenBin = hex2bin($validator);
            //Checks if passwords current hash is matching with the current option.
            $tokenCheck = password_verify($tokenBin, $row["pwdResetToken"]);

            //Checks if hash matches.
            if ($tokenCheck === false) {
                echo "You need to re-submit your request ";
                exit();
            } else if ($tokenCheck === true) {
                $tokenEmail = $row['pwdResetEmail'];

                //Gets everything from user with matching emails as the one that requested a password reset.
                $sql = "SELECT * FROM sc_users WHERE email=?;";
                $stmt = mysqli_stmt_init($mysqli_connection);
                if (!mysqli_stmt_prepare($stmt, $sql)) {
                    echo "There was an error! ";
                    exit();
                } else {
                    //We bind variabels to a prepared statement.
                    mysqli_stmt_bind_param($stmt, "s", $tokenEmail);
                    mysqli_stmt_execute($stmt);
                    $result = mysqli_stmt_get_result($stmt);
                    if (!$row = mysqli_fetch_assoc($result)) {
                        echo "There was an error! ";
                        exit();
                    } else {

                        //Updates password.
                        $sql = "UPDATE sc_users SET password=? WHERE email=?";
                        if (!mysqli_stmt_prepare($stmt, $sql)) {
                            echo "There was an error! ";
                            exit();
                        } else {
                            $newPwdHash = password_hash($password, PASSWORD_DEFAULT);
                            mysqli_stmt_bind_param($stmt, "ss", $newPwdHash, $tokenEmail);
                            mysqli_stmt_execute($stmt);

                            //Deletes the pswreset token
                            $sql = "DELETE FROM pwdReset WHERE pwdResetEmail=?";
                            $stmt = mysqli_stmt_init($mysqli_connection);
                            if (!mysqli_stmt_prepare($stmt, $sql)) {
                                echo "There was an error! ";
                                exit();
                            } else {
                                mysqli_stmt_bind_param($stmt, "s", $tokenEmail);
                                mysqli_stmt_execute($stmt);
                                header("location: /login.php?newpwd=passwordupdated");
                            }

                        }
                    }
                }
            }
        }
    }


} else {
    header("location: login.php");
}
