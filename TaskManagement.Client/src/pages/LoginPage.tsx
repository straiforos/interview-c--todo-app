import { useState } from 'react';
import { useNavigate, Link } from 'react-router';
import { authService } from '../services/auth.service';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Card, CardHeader, CardTitle, CardContent } from '@/components/ui/card';
import { CheckSquare, Loader2 } from 'lucide-react';
import { LanguageSelect } from '@/components/LanguageSelect';
import { useLingui } from '@lingui/react/macro';

export function LoginPage() {
  const { t } = useLingui();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    setError('');
    
    authService.login({ email, password }).subscribe({
      next: () => navigate('/tasks'),
      error: (err) => {
        setError(err.message || t({ id: 'login.error_failed', message: 'Login failed' }));
        setIsLoading(false);
      }
    });
  };

  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-muted/50 p-4 relative">
      <div className="absolute top-4 right-4">
        <LanguageSelect />
      </div>
      <Card className="w-full max-w-md">
        <CardHeader className="space-y-1 text-center">
          <div className="flex justify-center mb-4">
            <CheckSquare className="h-10 w-10 text-primary" />
          </div>
          <CardTitle className="text-2xl font-bold">{t({ id: 'login.title', message: 'Welcome back' })}</CardTitle>
          <p className="text-sm text-muted-foreground">
            {t({ id: 'login.subtitle', message: 'Enter your email to sign in to your account' })}
          </p>
        </CardHeader>
        <CardContent>
          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="space-y-2">
              <label className="text-sm font-medium leading-none" htmlFor="email">
                {t({ id: 'login.email', message: 'Email' })}
              </label>
              <Input
                id="email"
                type="email"
                placeholder={t({ id: 'login.email_placeholder', message: 'm@example.com' })}
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>
            <div className="space-y-2">
              <label className="text-sm font-medium leading-none" htmlFor="password">
                {t({ id: 'login.password', message: 'Password' })}
              </label>
              <Input
                id="password"
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>
            {error && <p className="text-sm text-destructive font-medium">{error}</p>}
            <Button className="w-full" type="submit" disabled={isLoading}>
              {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
              {t({ id: 'login.submit', message: 'Sign In' })}
            </Button>
          </form>
          <div className="mt-4 text-center text-sm">
            {t({ id: 'login.no_account', message: "Don't have an account?" })} {' '}
            <Link to="/register" className="text-primary hover:underline">
              {t({ id: 'login.sign_up', message: 'Sign up' })}
            </Link>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
