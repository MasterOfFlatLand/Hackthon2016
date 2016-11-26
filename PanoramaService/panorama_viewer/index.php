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
<?php include "inquiries.php";?>
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
$img_url_prefix = 'http://' . get_server_ip() . '/panorama_viewer/viewer.php?img=';
$pic_url_prefix = 'http://' . get_server_ip() . '/panorama_viewer/picture.php?img=';
?>
<style type="text/css">
body{ text-align: center; margin-top: 0px; margin-bottom: 0px; margin-left: 0px; margin-right: 0px; padding: 0; }
.parent{ text-align: center; width: 100%; height: 450px; }
.left{ text-align: center; width: 35%; height: 450px;float:left; }
.right{ text-align: center; width: 62%; height: 450px;float:left; }
.item{ width: 256px; height: 256px; }
</style>
<body>
<script type="text/javascript" src="js/jquery.min.js"></script>
<script type="text/javascript" src="js/jquery.qrcode.js"></script>
<script type="text/javascript" src="js/qrcode.js"></script>
<script type="text/javascript" src="dist/skippr.js"></script>
<br>
<div>微信扫一扫即可访问 VR 效果图</div>
<br>
<div class="parent">
	<div class="left">
		<?php echo "<div title='" . get_latest_pic() . "' id='latest_pic'></div>"; ?>
		<br><br>
		<?php echo "<div title='" . get_latest_img() . "' id='latest'></div>"; ?>
		<br>
		<div id='latest_txt'>
			<?php echo "<b>" . pathinfo(get_latest_img(), PATHINFO_FILENAME) . " 上-效果图 下-全景图 </b>" . " Time:" . get_img_time(get_latest_img()); ?>
		</div>
	</div>
	<div class="right" id="random">
		<?php
			foreach (get_top_imgs_except_first() as $image) {
				echo "<div class='item' title='" . $image . "' id='" . pathinfo($image, PATHINFO_FILENAME) . "'></div>";
			}
		?>
	</div>
</div>

<script>
function clocker() {
	var latest_pic = $('#latest_pic').attr("title");
	$.ajax({
			type: 'POST',
			url : 'interval.php',
			data: '',
			success: function(data) {
				if (latest_pic != data) {
					window.location.reload();
			}
		}
	});
}
</script>

<?php
echo "<script>";
echo "$(document).ready(function() {";
	echo "jQuery('#latest_pic').qrcode('" . $pic_url_prefix . get_latest_pic() . "');";
	echo "jQuery('#latest').qrcode('" . $img_url_prefix . get_latest_img() . "');";
	foreach (get_top_imgs_except_first() as $image) {
		echo "jQuery('#" . pathinfo($image, PATHINFO_FILENAME) . "').qrcode('" . $img_url_prefix . $image . "');";
	}
	echo "var interval = self.setInterval('clocker()', 10000);";
	echo "$('#random').skippr({";
		echo "transition: 'fade',";
		echo "autoPlay: true,";
		echo "autoPlayDuration: 5000,";
		echo "easing:'easeInOutCubic',";
		echo "logs: true";
	echo "});";
echo "});";
echo "</script>";
?>
</body>
</html>
