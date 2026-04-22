import React, { useEffect, useState } from 'react';
import { Card, CardHeader, CardTitle, CardContent, CardDescription } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { authService } from '../services/auth.service';
import { AuthResponse } from '../types';
import { Shield, User, Key } from 'lucide-react';
import { useLingui } from '@lingui/react/macro';

export const ProfilePage = () => {
  const { t } = useLingui();
  const [user, setUser] = useState<AuthResponse | null>(null);
  const [isSwapping, setIsSwapping] = useState(false);

  useEffect(() => {
    const sub = authService.currentUser$.subscribe(u => setUser(u));
    return () => sub.unsubscribe();
  }, []);

  const handleSwapRole = (roleName: string) => {
    if (!user || user.role === roleName) return;
    setIsSwapping(true);
    authService.swapRole(roleName).subscribe({
      next: () => {
        setIsSwapping(false);
      },
      error: (err) => {
        console.error(err);
        setIsSwapping(false);
      }
    });
  };

  if (!user) return null;

  const availableRoles = ['Admin', 'User', 'ReadOnly'];

  return (
    <div className="max-w-4xl mx-auto space-y-6">
      <div>
        <h1 className="text-3xl font-bold tracking-tight">{t({ id: 'profile.title', message: 'Profile Settings' })}</h1>
        <p className="text-muted-foreground">
          {t({ id: 'profile.subtitle', message: 'Manage your account and view permissions.' })}
        </p>
      </div>

      <div className="grid gap-6 md:grid-cols-2">
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <User className="h-5 w-5" />
              {t({ id: 'profile.account_details', message: 'Account Details' })}
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div>
              <p className="text-sm font-medium text-muted-foreground">Email</p>
              <p className="font-medium">{user.email}</p>
            </div>
            <div>
              <p className="text-sm font-medium text-muted-foreground">User ID</p>
              <p className="font-mono text-sm">{user.userId}</p>
            </div>
            <div>
              <p className="text-sm font-medium text-muted-foreground">Current Role</p>
              <div className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-semibold bg-primary/10 text-primary mt-1">
                {user.role}
              </div>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Key className="h-5 w-5" />
              {t({ id: 'profile.permissions', message: 'Active Permissions' })}
            </CardTitle>
            <CardDescription>
              {t({ id: 'profile.permissions_desc', message: 'The permissions granted by your current role.' })}
            </CardDescription>
          </CardHeader>
          <CardContent>
            {user.permissions && user.permissions.length > 0 ? (
              <ul className="space-y-2">
                {user.permissions.map(p => (
                  <li key={p} className="flex items-center gap-2 text-sm">
                    <Shield className="h-4 w-4 text-green-500" />
                    {p}
                  </li>
                ))}
              </ul>
            ) : (
              <p className="text-sm text-muted-foreground italic">No permissions granted.</p>
            )}
          </CardContent>
        </Card>

        <Card className="md:col-span-2 border-dashed border-2 border-primary/50 bg-primary/5">
          <CardHeader>
            <CardTitle className="text-primary flex items-center gap-2">
              <Shield className="h-5 w-5" />
              Demo Role Swapper
            </CardTitle>
            <CardDescription className="text-primary/80">
              This is a development-only feature to quickly test different permission levels.
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="flex flex-wrap gap-4">
              {availableRoles.map(role => (
                <Button
                  key={role}
                  variant={user.role === role ? 'default' : 'outline'}
                  onClick={() => handleSwapRole(role)}
                  disabled={isSwapping || user.role === role}
                  className="min-w-[120px]"
                >
                  {isSwapping && user.role !== role ? 'Swapping...' : `Switch to ${role}`}
                </Button>
              ))}
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
};
