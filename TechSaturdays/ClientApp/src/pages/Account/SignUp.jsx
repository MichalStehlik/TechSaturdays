import { useForm } from "react-hook-form"
import axios from "axios"
import { useNavigate } from "react-router-dom"

export const SignUp = () => {
    const { register, handleSubmit, watch, formState: { errors } } = useForm();
    const onSubmit = data => {
        axios.post("/api/v1/Account/register",
        {
            firstname: data.firstname,
            lastname: data.lastname,
            email: data.email,
            birthdate: data.birthdate,
            school: data.school,
            grade: Number(data.grade),
            aspirant: Boolean(data.aspirant),
            inmailinglist: Boolean(data.inmailinglist),
            password: data.password
        }
        )
        .then(response => {
            console.log(response.data);
            navigate("/account/sign-in");
        })
        .catch(error => {
            console.error(error);
        })
    };
    const navigate = useNavigate();
    return (
        <>
        <h1>Register</h1>
        <form onSubmit={handleSubmit(onSubmit)}>
            <div>
                <label>Firstname</label>
                <input defaultValue="" {...register("firstname")} />
            </div>
            <div>
                <label>Lastname</label>
                <input defaultValue="" {...register("lastname")} />
            </div>
            <div>
                <label>Email</label>
                <input defaultValue="" type="email" {...register("email")} />
            </div>
            <div>
                <label>Birth Date</label>
                <input defaultValue="" type="date" {...register("birthdate")} />
            </div>
            <div>
                <label>School</label>
                <input defaultValue="" {...register("school")} />
            </div>
            <div>
                <label>Grade</label>
                <input defaultValue="" {...register("grade")} />
            </div>
            <div>
                <label>Aspirant</label>
                <input defaultValue="" {...register("aspirant")} />
            </div>
            <div>
                <label>Mailing List</label>
                <input defaultValue="" {...register("inmailinglist")} />
            </div>
            <div>
                <label>Password</label>
                <input type="password" {...register("password")} />
            </div>
            <div>
                <label>Password again</label>
                <input type="password" {...register("passwordCopy")} />
            </div>
            <div>
                <button type="submit">Register</button>
            </div>
        </form>
        </>
    );
}

export default SignUp;