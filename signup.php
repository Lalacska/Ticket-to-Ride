
<!-- Her starter vores css! Man kunne også lave css i et individuelt men kan personligt godt lide at have det inde i mit php dokument. -->

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
  max-height: 400px;
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
}
.user-details .input-box input:focus{
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
    max-height: 300px;
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
    <title>Registration Page</title>
    <link rel="stylesheet" type="text/css" href="css/bootstrap.css" />
    <link rel="stylesheet" href="style.css">
  </head>
  <body>
        <body>
          <div class="container">
            <div class="title">Registration</div>
            <div class="content">
        
                <!-- post method to reach db (db = database) -->
              <form  action="register.php" method="post">
        
                <!-- core classes so css can style it. -->
                <div class="user-details">
                    <div class="input-box">
                    <span class="details">Username</span>
                    <input type="text" class="form-control" id="username" name="username" placeholder="Enter your username">
                  </div>

                  <div class="input-box">
                    <span class="details">Email</span>
                    <input type="text" class="form-control" id="email" name="email" placeholder="Enter your username">
                  </div>

                  <div class="input-box">
                    <span class="details">Password</span>
                    <input type="password1" class="form-control" id="password1" name="password1" placeholder="Enter your password" required autocomplete="off">
                  </div>

                  <div class="input-box">
                    <span class="details">Confirm Password</span>
                    <input type="password2" class="form-control" id="password2" name="password2" placeholder="Enter your password" required autocomplete="off">
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