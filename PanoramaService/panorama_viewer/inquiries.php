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
function print_latest_img() {
	echo get_latest_img();
}
?>
