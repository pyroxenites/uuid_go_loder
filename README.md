# uuid_go_loder
bypass AV

通过UUID的方式去bypass AV
只需要在cs生成veil格式的shellcode的txt文本即可

go build -ldflags "-w -s -H windowsgui" GO_UUID.go

生成32位,需要改变Go env
set GOARCH=386
