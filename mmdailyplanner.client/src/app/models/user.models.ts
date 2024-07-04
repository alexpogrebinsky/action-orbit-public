export interface User {
  id: number;
  username: string;
  firstName: string | null;
  lastName: string | null;
  addressIP: string | null;
  email: string;
  password: string;
  phoneNumber: string | null;
  role: string | null;
  isAuthenticated: boolean | null;
  profileImage: string | any[]; 
}
