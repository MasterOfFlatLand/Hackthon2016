<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtm
l1-transitional.dtd">
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="Author" content="">
<meta name="Keywords" content="">
<meta name="Description" content="">
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1"> 
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<title>扫描二维码浏览全景图</title>
<link rel="stylesheet" type="text/css" href="css/htmleaf-demo.css">
<link rel="stylesheet" href="assets/css/style.css">
<link rel="stylesheet" href="dist/skippr.css">
</head>
<?php
/**
* 获取服务器端IP地址
 * @return string
 */
function get_server_ip() {
    if (isset($_SERVER)) {
        if($_SERVER['SERVER_ADDR']) {
            $server_ip = $_SERVER['SERVER_ADDR'];
        } else {
            $server_ip = $_SERVER['LOCAL_ADDR'];
        }
    } else {
        $server_ip = getenv('SERVER_ADDR');
    }
    return $server_ip;
}
$url_prefix = 'http://' . get_server_ip() . '/panorama_viewer/viewer.php?img=';
?>
<?php
function get_top_ten_imgs() {
	$directory = "../panorama_imgs/";
	$images = glob($directory . '*.{jpg,JPG,jpeg,JPEG,png,PNG}', GLOB_BRACE);
	usort($images, function($a, $b) {
		return filemtime($a) < filemtime($b);
	});
	
	$files = array();       
	foreach ($images as $image) {
		array_push($files, basename($image));
	}
	return array_slice($files, 0, 10);// top 10.
}
function get_latest_img() {
	$imgs = get_top_ten_imgs();
	return reset($imgs);
}
$latest_img = get_latest_img();
function is_added_new_img() {
	global $latest_img;
	$pre_latest_img = $latest_img;
	$latest_img = get_latest_img();
	if (strcmp($latest_img, $pre_latest_img) == 0) {
		return false;
	}
	return true;
}
?>
<style type="text/css">
body{ text-align: center; margin-top: 0px; margin-bottom: 0px; margin-left: 0px; margin-right: 0px; padding: 0; }
.parent{ text-align: center; width: 100%; height: 450px; }
.item{ width: 256px; height: 256px; }
</style>
<body>
<script type="text/javascript" src="js/jquery.min.js"></script>
<script type="text/javascript" src="js/jquery.qrcode.js"></script>
<script type="text/javascript" src="js/qrcode.js"></script>
<script type="text/javascript" src="dist/skippr.js"></script>
<br>
<div class="parent">
	<div>打开微信扫一扫即可访问 VR 全景图。<?php echo !is_added_new_img();?></div>
	<br>
	<div class="parent" id="random">
		<?php
			foreach (get_top_ten_imgs() as $image) {
				echo "<div class='item' title='" . $image . "' id='" . pathinfo($image, PATHINFO_FILENAME) . "'></div>";
			}
		?>
	</div>
</div>

<script>
function clocker() {
	//var need_reload = <?php echo is_added_new_img();?>;
	if (/*need_reload == */false) {
		window.location.reload();
	}
}
</script>

<?php
echo "<script>";
echo "$(document).ready(function() {";
	foreach (get_top_ten_imgs() as $image) {
		echo "jQuery('#" . pathinfo($image, PATHINFO_FILENAME) . "').qrcode('" . $url_prefix . $image . "');";
	}
	echo "var interval = self.setInterval('clocker()', 3000);";
	echo "$('#random').skippr({";
		echo "transition: 'fade',";
		echo "autoPlay: true,";
		echo "autoPlayDuration: 3000,";
		echo "easing:'easeInOutCubic',";
		echo "logs: true";
	echo "});";
echo "});";
echo "</script>";
?>
</body>
</html>
