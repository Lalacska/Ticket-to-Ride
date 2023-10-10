<?php
session_start();

require('connection.php');

//If username isnt typed you will get an error.
if (!isset($_SESSION['username'])) {
  echo "You are not logged in";
} else {

}

//Get all info from the user that logged in.
$strSQL = "SELECT * FROM sc_users WHERE email = '" . $_SESSION['username'] . "'";

$rs = mysqli_query($mysqli_conection, $strSQL);

//Post all the info into rows.
while ($row = mysqli_fetch_array($rs)) {
  $username = $row["username"];
  $Useremail = $row['email'];
  $pass = $row['password'];
  $regdate = $row['registration_date'];
}

?>


<!-- Css starts -->

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

  .container {
    max-width: 700px;
    max-height: 500px;
    height: 100%;
    width: 100%;
    background-color: #fff;
    padding: 25px 30px;
    border-radius: 5px;
    box-shadow: 0 5px 10px rgba(0, 0, 0, 0.15);
    background-color: #191919;
  }

  .container .title {
    font-size: 25px;
    font-weight: 500;
    position: relative;
    color: #ff785a;
  }

  .container .title::before {
    content: "";
    position: absolute;
    left: 0;
    bottom: 0;
    height: 3px;
    width: 30px;
    border-radius: 5px;
    background: linear-gradient(135deg, #191919, #ff785a);
  }

  .content form .user-details {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    margin: 20px 0 12px 0;
    color: #ff785a;
  }

  form .user-details .input-box {
    margin-bottom: 15px;
    width: calc(100% / 2 - 20px);
  }

  .user-details .input-box input {
    height: 45px;
    width: 100%;
    outline: none;
    font-size: 16px;
    border-radius: 5px;
    padding-left: 15px;
    border: 1px solid #ccc;
    border-bottom-width: 2px;
    transition: all 0.3s ease;
    font-size: 20px;
  }

  .user-details .input-box input:focus {
    border-color: #ff785a;
  }

  form .button {
    display: flexbox;
    margin-top: 40px;
    margin-left: auto;
    height: 50px;
    max-width: 700px;
    width: 90%;
  }

  form .button input {
    height: 100%;
    width: 90%;
    border-radius: 5px;
    border: none;
    color: #fff;
    font-size: 18px;
    font-weight: 500;
    letter-spacing: 1px;
    cursor: pointer;
    transition: all 0.3s ease;
    background-color: #ff785a;
  }

  form .button input:hover {
    background-color: #f74017;
  }

  @media (max-width: 584px) {
    .container {
      max-width: 100%;
    }

    form .user-details .input-box {
      margin-bottom: 15px;
      width: 100%;
    }

    form .category {
      width: 100%;
    }

    form .register {
      margin-right: 60px;
    }

    .content form .user-details {
      max-height: 500px;
      overflow-y: scroll;
    }

    .user-details::-webkit-scrollbar {
      width: 5px;
    }
  }
</style>

<!-- Html starts -->

<html>

<head>
  <title>Profile Page</title>
  <link rel="stylesheet" type="text/css" href="css/bootstrap.css" />
  <link rel="stylesheet" href="style.css">
</head>

<body>

  <body>
    <div class="container">
      <div class="title">Profile</div>
      <div class="content">

        <!-- post method to reach db (db = database) -->
        <form action="profile.php" method="post">

          <!-- core classes so css can style it. -->
          <div class="user-details">
            <div class="input-box">
              <span class="details">Current Username</span>
              <input type="text" class="form-control" id="username" name="username" readonly
                placeholder="<?php print "{$username}" ?>">
            </div>

            <div class="input-box">
              <span class="details">New Username</span>
              <input type="text" class="form-control" id="nu" name="email" placeholder="New Username">
            </div>

            <div class="user-details">
              <div class="input-box">
                <span class="details">Current Email</span>
                <input type="text" class="form-control" id="ce" name="username" readonly
                  placeholder="<?php print "{$Useremail}" ?>">
              </div>

              <div class="input-box">
                <span class="details">New Email</span>
                <input type="text" class="form-control" id="email" name="email" placeholder="New Email">
              </div>

              <div class="input-box">
                <span class="details">New Password</span>
                <input type="password1" class="form-control" id="password1" name="password1"
                  placeholder="Enter your password" required autocomplete="off">
              </div>

              <div class="input-box">
                <span class="details">Confirm Password</span>
                <input type="password2" class="form-control" id="password2" name="password2"
                  placeholder="Enter your password" required autocomplete="off">
              </div>

              <div class="button">
                <input id="button" type="submit" value="Register"><br><br>
                <a href="https://bekbekbek.com/login.php"></a><br><br>
              </div>
            </div>
        </form>
      </div>
    </div>
    </div>
  </body>

</html>

