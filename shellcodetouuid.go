package main

import (
	"bufio"
	"bytes"
	"encoding/binary"
	"encoding/hex"
	"flag"
	"fmt"
	"github.com/google/uuid"
	"io/ioutil"
	"os"
	"strings"
)

var(

	src_file string
	des_file string

)
func shellcode_to_uuids(shellcode []byte) ([]string, error) {
	// Pad shellcode to 16 bytes, the size of a UUID
	if 16-len(shellcode)%16 > 16 {
		pad := bytes.Repeat([]byte{byte(0x90)}, 16-len(shellcode)%16)
		shellcode = append(shellcode, pad...)
	}

	var uuids []string

	for i := 0; i < len(shellcode); i += 16 {
		var uuidBytes []byte

		// This seems unecessary or overcomplicated way to do this

		// Add first 4 bytes
		buf := make([]byte, 4)
		binary.LittleEndian.PutUint32(buf, binary.BigEndian.Uint32(shellcode[i:i+4]))
		uuidBytes = append(uuidBytes, buf...)

		// Add next 2 bytes
		buf = make([]byte, 2)
		binary.LittleEndian.PutUint16(buf, binary.BigEndian.Uint16(shellcode[i+4:i+6]))
		uuidBytes = append(uuidBytes, buf...)

		// Add next 2 bytes
		buf = make([]byte, 2)
		binary.LittleEndian.PutUint16(buf, binary.BigEndian.Uint16(shellcode[i+6:i+8]))
		uuidBytes = append(uuidBytes, buf...)

		// Add remaining
		uuidBytes = append(uuidBytes, shellcode[i+8:i+16]...)

		u, err := uuid.FromBytes(uuidBytes)
		if err != nil {
			return nil, fmt.Errorf("there was an error converting bytes into a UUID:\n%s", err)
		}

		uuids = append(uuids, u.String())
	}
	return uuids, nil
}
func main(){
	flag.StringVar(&src_file,"s","","请输入一下你的源文件")
	flag.StringVar(&des_file,"d","","请你输入你想生成的目标文件")
	flag.Parse()
	data,err:=ioutil.ReadFile(src_file)
	if err!=nil{
		fmt.Println("veil文件打开出错")
	}
	string_shellcode:=string(data)
	replace_shellcode:=strings.Replace(string_shellcode,"\\x","",-1)
	shellcode, _ := hex.DecodeString(replace_shellcode)

	uuids, _ := shellcode_to_uuids(shellcode)
	filePath:=des_file
	file, err := os.OpenFile(filePath, os.O_WRONLY|os.O_CREATE, 0666)
	if err != nil {
		fmt.Println("文件打开失败", err)
	}
	defer file.Close()
	write := bufio.NewWriter(file)
	for _,i := range uuids{
		write.WriteString("\""+i+"\""+",")
	}
	//Flush将缓存的文件真正写入到文件中
	write.Flush()

}