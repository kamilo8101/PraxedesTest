import Swal from "sweetalert2";

export class SweetAlertUtils {
  
  public static SwallQuestion(text: string, callBack: Function) {
    Swal.fire({
      text: text,
      icon: "question",
      allowOutsideClick: false,
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Si",
      cancelButtonText: "No",
    }).then((result) => {
      callBack(result.isConfirmed);
    });
  }

  public static SwallSucces(title: string = "¡Correcto!", text: string, callBack: Function | null = null) {
    Swal.fire({
      title: title,
      text: text,
      icon: "success",
    }).then((result) => {
      if (callBack) callBack(result.isConfirmed);
    });;
  }

  public static SwallErro(text: string) {
    Swal.fire({
      icon: "error",
      title: "Oops...",
      text: text,
      footer: "",
    });
  }

  public static SwalDetails(title: string, text: string, isXML: boolean) {
    if (isXML) {
      Swal.fire({
        title: title,
        text: `<div class="align-left" style="text-align: left;white-space: pre;">${text}</div>`,
        width: 1500,
        footer: "",
      });
    } else {
      Swal.fire({
        title: title,
        html: `<div class="align-left" style=" text-align: left;">${text}</div>`,
        width: 1500,
        footer: "",
      });
    }
  }
}
