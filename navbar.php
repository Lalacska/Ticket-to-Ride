<?php
// Require the connection file
require('connection.php');

// Check if the user is logged in
if (!isset($_SESSION['user_id'])) {
    // If not logged in, display this navigation bar
?>
    <link rel="stylesheet" href="styles/navbar-style.css">
    <nav class="navbar navbar-expand-lg navbar-light">
        <div class="container-fluid">
            <a class="navbar-brand" routerLink=""></a>
            <!-- Button for mobile view -->
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNavAltMarkup">
                <div class="navbar-nav ms-auto">
                    <a href="login.php">Login</a>
                    <a href="contact.php">Contact</a>
                </div>
            </div>
        </div>
    </nav>

<?php
} else {
    // If logged in, display this navigation bar
?>
    <link rel="stylesheet" href="styles/navbar-style.css">
    <nav class="navbar navbar-expand-lg navbar-light">
        <div class="container-fluid">
            <a class="navbar-brand" routerLink=""></a>
            <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNavAltMarkup" aria-controls="navbarNavAltMarkup" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarNavAltMarkup">
                <div class="navbar-nav ms-auto">
                    <a class="nav-link" href="forum.php">Forum</a>
                    <a class="nav-link" href="profile_page.php">Profile</a>
                    <a class="nav-link" href="contact.php">Contact</a>
                    <a href="logout.php">Logout</a>
                </div>
            </div>
        </div>
    </nav>

<?php
}
?>