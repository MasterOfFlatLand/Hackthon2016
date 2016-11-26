<!doctype html>
<html lang="en">
 <head>
  <meta charset="UTF-8">
  <meta name="Author" content="">
  <meta name="Keywords" content="">
  <meta name="Description" content="">
  <title>Protocol</title>
 </head>
 <body>
<?php
function RenderTask()
{
	$curtime = time();
	$target = "  \"string filename\" [\"C:/Apache24/htdocs/viewer/examples/custom-contextmenu/"."BigBang".$curtime."\"]";
	$file = "B:/Temp/Target".$curtime.".lxt";
	$h = fopen($file, "w");
	fwrite($h, $target);
	fclose($h);

	$context = new ZMQContext(1);
	// Socket to talk to clients
	$socket = new ZMQSocket($context, ZMQ::SOCKET_REQ);
	$socket->connect("tcp://localhost:5555");
	$socket->send("Hello|".$file);
	$response = $socket->recv();
	printf ("Received response: [%s]\n", $response);
}

function ProcessVector($oxyz, $ixyz)
{
	$oxyz[0] = -$ixyz[0];
	$oxyz[1] = -$ixyz[2];
	$oxyz[2] = $ixyz[1];
}

function ProcessMatrix($trans)
{
	//return $trans;
	$im = explode(" ", $trans);
	$om = array();
	$om[0]  = $im[0]; $om[1]  = $im[4]; $om[2]  = $im[8] ; $om[3]  = $im[12];
	$om[4]  = $im[1]; $om[5]  = $im[5]; $om[6]  = $im[9] ; $om[7]  = $im[13];
	$om[8]  = $im[2]; $om[9]  = $im[6]; $om[10] = $im[10]; $om[11] = $im[14];
	$om[12] = $im[3]; $om[13] = $im[7]; $om[14] = $im[11]; $om[15] = $im[15];
	return implode(" ", $om);
}

function ProcessCamera($h)
{
	if(empty($_REQUEST['camera']))
		return;

	$camera = $_REQUEST['camera'];
	echo "  <p>\nCamera: ";
	print $camera;

	fwrite($h, "camera:\n");
	fwrite($h, $camera);

	#$camera = '0 0.65 0 -0.009393859 0 -0.9999559 0 1 0';
	$campara = explode(' ', $camera);

	// camera position
	$cpx = -$campara[0];
	$cpy = -$campara[2];
	$cpz = $campara[1];

	// camera lookat point ( target )
	$cdx = -$campara[3];
	$cdy = -$campara[5];
	$cdz = $campara[4];
	bcscale(10);
	$ctx = bcadd($cpx, $cdx);
	$cty = bcadd($cpy, $cdy);
	$ctz = bcadd($cpz, $cdz);

	// camera up direction
	$cux = -$campara[6];
	$cuy = -$campara[8];
	$cuz = $campara[7];

	$cam_out = array(
		$cpx, $cpy, $cpz,
		$ctx, $cty, $ctz,
		$cux, $cuy, $cuz
		);

	$handle = fopen("B:/Temp/Camera.lxc", "w");
	fwrite($handle, "LookAt ");
	fwrite($handle, implode(' ', $cam_out));
	fclose($handle);
}

function ProcessModel($model, $handle)
{
	$pieces = explode(",", $model);
	$count = count($pieces);
	if($count < 2)
		return;

	$idmodel = $pieces[0];
	echo "<br />\nModel ID: ";
	print $idmodel;
	$trans = $pieces[1];
	echo "<br />\nTransform: ";
	print $trans;
	echo "<br />\n";

	$needremove = array("[", "]");
	$tranformi = str_replace($needremove, "", $trans);
	$tranform = ProcessMatrix($tranformi);
	fwrite($handle, "TransformBegin\n");
	//fwrite($handle, "ConcatTransform [".$tranform."]\n");
	fwrite($handle, "Transform [".$tranform."]\n");
	fwrite($handle, "ObjectInstance \"".$idmodel."\"\n");
	fwrite($handle, "TransformEnd\n\n");
}

function ProcessModels($h)
{
	if(empty($_REQUEST['models']))
		return;

	$modelp = $_REQUEST['models'];
	echo "<br /><br />\nModels: <br />\n";

	fwrite($h, "\nmodels:\n");
	fwrite($h, $modelp);

	$models = explode(";", $modelp);

	$handle = fopen("B:/Temp/Furnitures.lxf", "w");

	foreach($models as $model) {
		ProcessModel($model, $handle);
	}

	fclose($handle);
	echo "  </p>\n";
}

#print phpinfo();

/*
$headers = apache_request_headers();
foreach ($headers as $header => $value) {
    echo "$header: $value <br />\n";
}
*/

#print_r($_REQUEST);
$dlookat = '3.876106 -4.371810 7.572081 3.535682 -3.937974 6.737875 -0.499441 0.668262 0.551348';
$transform0 = '[-0.006672079674900 0.000000001007456 0.000000000000000 0.000000000000000 -0.000000001007456 -0.006672079209238 0.000000000000000 0.000000000000000 0.000000000000000 0.000000000000000 0.068586871027946 0.000000000000000 0.836706995964050 1.845357656478882 -0.614888012409210 1.000000000000000]';
$model0 = 'model_id0';
$transform1 = '[0.006672079674900 0.000000001007456 0.000000000000000 0.000000000000000 -0.000000001007456 -0.006672079209238 0.000000000000000 0.000000000000000 0.000000000000000 0.000000000000000 0.068586871027946 0.000000000000000 0.836706995964050 1.845357656478882 -0.614888012409210 1.000000000000000]';
$model1 = 'model_id1';
$dmodel = $model0.','.$transform0.';'.$model1.','.$transform1;
#$dlookat = '0 1.6 0 0 0 -1 0 1 0';
#$dlookat = '-0.83 1.4 -1.8 0 0 -1 0 1 0'; //f
#$dlookat = '-0.83 1.4 -1.8 0 0 1 0 1 0'; //b
#$dlookat = '-0.83 1.4 -1.8 0 1 0 1 0 0'; //u
#$dlookat = '-0.83 1.4 -1.8 0 -1 0 -1 0 0'; //d
#$dlookat = '-0.83 1.4 -1.8 1 0 0 0 1 0'; //r
$dlookat = '-0.83 1.4 -1.8 -1 0 0 0 1 0'; //l
$dmodel = 'LongSofa,[1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1];ShortSofa,[1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1];Case,[1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1];Table,[1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1];KitchTable,[1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1];Desk,[1 0 0 0 0 1 0 0 0 0 1 0 0 0 0 1]';

if(count($_REQUEST) > 0) {
	$h = fopen("B:/params.log", "w");

	ProcessCamera($h);

	ProcessModels($h);

	fclose($h);

	RenderTask();
} else {
?>
  <form action="index.php" method="post">
   Camera:<br /><input type="text" name="camera" value="<?php echo $dlookat; ?>" style="width:1024px" /><br /><br />
   Model:<br /><textarea name="models" cols=40 rows=12 style="width:1024px"><?php echo $dmodel; ?></textarea><br />
   <input type="submit" value="提交" /><br />
  </form>
  <p>
    <br />
	<a href="/viewer/krpano.html?xml=examples/custom-contextmenu/contextmenu.xml">CubeMap</a>
	<br />
说明：<br /><br />
相机LookAt(同OpenGL的gluLookAt) float*9:<br />
  float float float float float float float float<br /><br />
4*4变换矩阵 float*16:<br />
  float float float float float float float float float float float float float float<br /><br />
camera: float*9<br /><br />
model: model_id,[float*16]<br /><br />
models: model;model;...
  </p>
<?php
}
?>
 </body>
</html>
