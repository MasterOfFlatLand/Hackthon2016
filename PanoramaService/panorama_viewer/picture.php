<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtm
l1-transitional.dtd">
<html lang="en">
<head>
<meta charset="UTF-8">
<meta name="Author" content="">
<meta name="Keywords" content="">
<meta name="Description" content="">
<title>VR Fantasy</title>
</head>
<?php $val_overview_pic = $_GET['img']; ?>
<style type="text/css">
body{ margin-top: 0px; margin-bottom: 0px; margin-left: 0px; margin-right: 0px; padding: 0; }
</style>
<body>
<script type="text/javascript" src="js/jquery.min.js"></script>
<script src="js/three.min.js"></script>
<script src="js/photo-sphere-viewer.min.js"></script>
<div style="display:table-cell; vertical-align:middle; text-align:center" id="container">
	<img src='../panorama_pics/<?php echo $val_overview_pic?>'>
</div>
<script type="text/javascript">
$(document).ready(function(){
	document.title = 'VR 效果图 - <?php echo $val_overview_pic?>';
});
</script>
</body>
</html>
