export interface UserProfileDto {
  username: string;
  firstName: string | null;
  lastName: string | null;
  email: string;
  phoneNumber: string | null;
  profileImage: string | any[];
}
