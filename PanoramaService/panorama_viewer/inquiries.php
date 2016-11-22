<?php
function get_imgs_dir() {
	return "../panorama_imgs/";
}
function get_top_imgs() {
	$directory = get_imgs_dir();
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
function get_img_time($input) {
	return date("Y-m-d H:i:s", filemtime(get_imgs_dir() . $input));
}
function get_latest_img() {
	$imgs = get_top_imgs();
	return reset($imgs);
}
function get_top_imgs_except_first() {
	$imgs = get_top_imgs();
	return array_slice($imgs, 1);
}
function print_latest_img() {
	echo get_latest_img();
}
?>
