export class User {
  userName: string;
  password: string;
  gender: string;
  dateOfBirth: Date;
  knownAs: string;
  introduction: string;
  lookingFor: string;
  interests: string;
  city: string;
}
export class Login {
  userName: string;
  password: string;
  remember?: boolean;
}
export class Signup extends User {
  id: number;
}
