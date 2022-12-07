export const API_URL = "http://localhost:5247/api";
export const TOKEN = localStorage.getItem("token");
export const DETOKEN = JSON.parse(localStorage.getItem("deToken"));
export const HEADERS_MULTIPART_CONFIG = {
  Authorization: `bearer ${TOKEN}`,
  "content-type": "multipart/form-data",
};
export const HEADERS_CONFIG = { Authorization: `bearer ${TOKEN}` };
export const WEB_URL = "http://localhost:5192";
export const SERVER_URL = "http://localhost:5247/"
export const CATEGORY_MANAGEMENT_URL = API_URL + "/CategoryManagement";
export const POST_MANAGEMENT_URL = API_URL + "/PostManagement";
export const IMAGE_MANAGEMENT_URL = API_URL + "/ImageManagement";
export const IMAGECATEGORY_MANAGEMENT_URL =
  API_URL + "/ImageCategoryManagement";
export const REPORT_MANAGEMENT_URL = API_URL + "/ReportManagement";
export const POST_URL = API_URL + "/Post";
export const HOME_URL = API_URL + "/Home";
export const RATE_URL = API_URL + "/Rate";
export const CATEGORY_URL = API_URL + "/Category";
export const COMMENT_URL = API_URL + "/Comment";
export const LOGIN_API_URL = API_URL + "/Authen/Login";
export const REGISTER_API_URL = API_URL + "/Authen/Register";
export const AUTHEN_API_URL = API_URL + "/Authen"