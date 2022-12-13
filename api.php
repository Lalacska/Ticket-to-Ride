<?php 
require_once __DIR__ . '/api_config.php';

class API {
    function select(){
        $db = new Connect;
        $users = array();
        $data = $db->prepare('SELECT user_id, username, email, registration_date FROM sc_users ORDER BY user_id');
        $data->execute();
        while($OutputData = $data->fetch(PDO::FETCH_ASSOC)){
            $users[$OutputData['user_id']] = array(
                'user_id' => $OutputData['user_id'],
                'username' => $OutputData['username'],
                'email' => $OutputData['email'],
                'registration_date' => $OutputData['registration_date']
            );
        }
        return json_encode($users);
    }
}

$API = new API;
header('Content-Type: application/json');
echo $API->select();

?>