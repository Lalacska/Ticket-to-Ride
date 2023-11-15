<?php
session_start();

// Your PHP code
if (isset($_SESSION['user_id'])) {
    echo 'loggedin';
} else {
    echo 'notloggedin';
}
?>