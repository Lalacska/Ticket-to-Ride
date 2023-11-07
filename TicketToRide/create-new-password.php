<!--Her er alt vores css. Dette bliver brugt til at ændre alt visuelt.-->

<style>
    * {
        margin: 0;
        padding: 0;
        box-sizing: border-box;
        font-family: "Poppins", sans-serif;
    }

    body {
        /* Her laver vi alt det visuelle til siden og gør den mobile compatibel med hjælp af flex og vh*/

        /* VH = view height. Dette gøt at den automatisk formindskes og forstørres. */
        height: 100vh;
        display: flex;
        justify-content: center;
        align-items: center;
        padding: 10px;
        background: linear-gradient(135deg, #191919, #ff785a);
        width: 100%;
    }

    .section-deafult {
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

    .title {
        font-size: 25px;
        font-weight: 500;
        position: relative;
        color: #ff785a;
        margin-left: 150px;
    }

    p {
        font-size: 11px;
        color: #ff785a;
        padding-top: 6px;
        margin-left: 160px;
    }

    .p2 {
        margin-left: 180;
    }

    .np {
        margin: 0%;
    }

    .rp {
        margin: 0%;
    }

    input {
        display: block;
        width: 550px;
        height: 15%;
        border-radius: 5px;
        margin-bottom: 25px;
    }

    button {
        width: 90%;
        height: 10%;
        margin-left: 25px;
        border-radius: 5px;
        background-color: #ff785a;
        color: #fff;
    }
</style>

<!-- Our html begins here! We use html to make the core of our website.-->
<main>
    <div class="wrapper-main">
        <section class="section-deafult">
            <?php

            //$selector is a global variable and we use it to get our old password and save 
            $selector = $_GET["selector"];
            //$validator is a global variable and we use it to save the users token. we use tokens to find the user.
            $validator = $_GET["validator"];

            //Check if empty
            if (empty($selector) || empty($validator)) {
                //If empty
                echo "No token and no bitches";
            } else {

                //ctyp_xdigit is to check if every character is hexadecimal. Such as all decimal numbers and chars from A-F & a-f. If not = false.
                if (ctype_xdigit($selector) !== false && ctype_xdigit($validator) !== false) {
            ?>

            <!--We use this code to update the old password to our new password-->
            <h1 class="title">Create New Password</h1>
            <p class="p1">This will be your new password to your account</p>
            <p class="p2">information is saved after you hit save</p>
            <form action="reset-password.inc.php" method="post">
                <input type="hidden" name="selector" value="<?php echo $selector ?>">
                <input type="hidden" name="validator" value="<?php echo $validator ?>">
                <p class="np">New password</p>
                <input type="password" name="pwd" placehoder="Enter a new password">
                <p class="rp">Repeat Password</p>
                <input type="password" name="pwd-repeat" placehoder="Repeat new password">
                <button type="submit" name="reset-password-submit">Reset password</button>
            </form>

            <?php
                }
            }
                        ?>
        </section>
    </div>
</main>
