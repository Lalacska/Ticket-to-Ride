<?php
// Start a session or resume the existing session
session_start();

// Check if the 'user_id' key is set in the session
if (isset($_SESSION['user_id'])) {
    // If 'user_id' is set, the user is considered logged in
    echo 'loggedin';
} else {
    // If 'user_id' is not set, the user is considered not logged in
    echo 'notloggedin';
}
?>
