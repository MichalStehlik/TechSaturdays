import { useForm } from "react-hook-form"
import axios from "axios"
import { useNavigate, useSearchParams } from "react-router-dom"

export const PasswordRecovery = () => {
    let [searchParams, setSearchParams] = useSearchParams();
    let email = searchParams.get("email");
    const { register, handleSubmit, watch, formState: { errors } } = useForm({defaultValues: {email: email}});
    const onSubmit = data => {
        axios.post("/api/v1/Account/sent-password-recovery",
        {
            email: data.email,
        }
        )
        .then(response => {
            navigate("/account/email-confirmation");
        })
        .catch(error => {
            console.error(error);
        })
    };
    const navigate = useNavigate();
    return (
        <>
        <h1>Password Recovery</h1>
        <form onSubmit={handleSubmit(onSubmit)}>
            <div>
                <label>Email</label>
                <input defaultValue="" type="email" {...register("email")} />
            </div>
            <div>
                <button type="submit">Send recovery email</button>
            </div>
        </form>
        </>
    );
}

export default PasswordRecovery;