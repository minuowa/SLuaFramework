
cd android/jni
./build_arm_v7.sh
cp ../libs/armeabi-v7a/libslua.so ../../../Assets/Plugins/Android/libs/armeabi-v7a/libslua.so
./build_x86.sh
cp ../libs/x86/libslua.so ../../../Assets/Plugins/Android/libs/x86/libslua.so
cd ../../

